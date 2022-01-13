using System;
using System.Linq;
using Domain.Interfaces;
using Domain.Models;
using Domain.Requests;
using Domain.Responses;

namespace Domain.UseCases
{
    public class UpdateUserUseCase : IUpdateUserUseCase
    {
        private readonly IUserGateway _userGateway;

        public UpdateUserUseCase(IUserGateway userGateway)
        {
            _userGateway = userGateway;
        } 

        public void Execute(UpdateUserRequest request, IPresenter presenter)
        {
            try
            {
                if (CheckValidation(request, presenter)) return;

                var user = _userGateway.Get(Guid.Parse(request.UserId));
                if (user == null)
                {
                    presenter.Error("Cannot find user to update");
                    return;
                }

                var updatedUserDto = new User
                {
                    Id = Guid.Parse(request.UserId),
                    Name = request.Name,
                    Surname = request.Surname,
                    EmailAddress = request.Email
                };

                var updatedUser = _userGateway.Update(updatedUserDto);
                presenter.Success(new UpdateUserResponse
                {
                    UpdatedUser = updatedUser
                });
            }
            catch (Exception e)
            {
                presenter.Error($"Unknown Error Occurred, {e.Message}");
                return;
            }
          
        }

        private bool CheckValidation(UpdateUserRequest request, IPresenter presenter)
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

            if (request.Email.Length < 3)
            {
                presenter.Error("Email must have at least 3 characters");
                return true;
            }

            if (!request.Email.Contains("@"))
            {
                presenter.Error("Invalid email address");
                return true;
            }

            var existingUsers = _userGateway.GetAll().Where(x => x.Id != Guid.Parse(request.UserId));
            if (existingUsers.Any(x => x.EmailAddress == request.Email))
            {
                presenter.Error("Duplicate email address");
                return true;
            }

            return false;
        }
    }
}