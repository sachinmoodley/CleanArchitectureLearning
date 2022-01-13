using System;
using System.Linq;
using Domain.Interfaces;
using Domain.Models;
using Domain.Requests;
using Domain.Responses;

namespace Domain.UseCases
{
    public class CreateUserUseCase : ICreateUserUseCase
    {
        private readonly IUserGateway _userGateway;

        public CreateUserUseCase(IUserGateway userGateway)
        {
            _userGateway = userGateway;
        } 

        public void Execute(CreateUserRequest request, IPresenter presenter)
        {
            if (CheckValidation(request, presenter)) return;
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Surname = request.Surname,
                EmailAddress = request.EmailAddress
            };

            _userGateway.Add(user);

            var password = new PasswordDto
            {
                Id = user.Id,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword
            };

            _userGateway.AddPassword(password);

            presenter.Success(new CreateUserResponse
            {
                Id = user.Id
            });
        }

        private bool CheckValidation(CreateUserRequest request, IPresenter presenter)
        {
            if (request.Name.Length < 1)
            {
                presenter.Error("Invalid name");
                return true;
            }

            if (request.Surname.Length < 1)
            {
                presenter.Error("Invalid surname");
                return true;
            }

            if (request.EmailAddress.Length < 3)
            {
                presenter.Error("Email must have at least 3 characters");
                return true;
            }

            if (!request.EmailAddress.Contains("@"))
            {
                presenter.Error("Invalid email address");
                return true;
            }

            var existingUsers = _userGateway.GetAll();
            if (existingUsers.Any(x => x.EmailAddress == request.EmailAddress))
            {
                presenter.Error("Duplicate email address");
                return true;
            }

            if (request.Password != request.ConfirmPassword)
            {
                presenter.Error("New Password and Confirm Passwords do not match");
                return true;
            }

            return false;
        }
    }
}