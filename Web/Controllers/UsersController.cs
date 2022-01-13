using System;
using Domain.Interfaces;
using Domain.Requests;
using Domain.UseCases;
using Microsoft.AspNetCore.Mvc;
using Web.Presenters;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IActionResultPresenter _presenter;
        private readonly ICreateUserUseCase _createUserUseCase;
        private readonly ICreateOtpUseCase _createOtpUseCase;
        private readonly IDeleteUserUseCase _deleteUserUseCase;
        private readonly IGetUserDetailsUseCase _getUserDetailsUseCase;
        private readonly IResetPasswordUseCase _resetPasswordUseCase;
        private readonly IUpdateUserUseCase _updateUserUseCase;
        private readonly IUserGateway _userGateway;

        public UsersController(
            IActionResultPresenter presenter,
            ICreateUserUseCase createUserUseCase,
            ICreateOtpUseCase createOtpUseCase, 
            IDeleteUserUseCase deleteUserUseCase, 
            IGetUserDetailsUseCase getUserDetailsUseCase, 
            IResetPasswordUseCase resetPasswordUseCase, 
            IUpdateUserUseCase updateUserUseCase, 
            IUserGateway userGateway)
        {
            _createUserUseCase = createUserUseCase;
            _presenter = presenter;
            _createOtpUseCase = createOtpUseCase;
            _deleteUserUseCase = deleteUserUseCase;
            _getUserDetailsUseCase = getUserDetailsUseCase;
            _resetPasswordUseCase = resetPasswordUseCase;
            _updateUserUseCase = updateUserUseCase;
            _userGateway = userGateway ?? throw new ArgumentNullException(nameof(userGateway));
        }

        [HttpPost]
        public IActionResult Add(CreateUserRequest request)
        {
            _createUserUseCase.Execute(request, _presenter);
            return _presenter.Render();
        }

        [HttpGet("otp")]
        public IActionResult CreateOtp(CreateOtpRequest request)
        {
            _createOtpUseCase.Execute(request, _presenter);
            return _presenter.Render();
        }

        [HttpDelete]
        public IActionResult Delete(DeleteUserRequest request)
        {
            _deleteUserUseCase.Execute(request, _presenter);
            return _presenter.Render();
        }

        [HttpGet("user")]
        public IActionResult Get(GetUserDetailsRequest request)
        {
            _getUserDetailsUseCase.Execute(request, _presenter);
            return _presenter.Render();
        }

        [HttpPut("user/password")]
        public IActionResult UpdatePassword(ResetPasswordRequest request)
        {
            _resetPasswordUseCase.Execute(request, _presenter);
            return _presenter.Render();
        }

        [HttpPut("user")]
        public IActionResult Update(UpdateUserRequest request)
        {
            _updateUserUseCase.Execute(request, _presenter);
            return _presenter.Render();
        }

        //this is just for project startup to show some info
        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var  users = _userGateway.GetAll();
            return new OkObjectResult(users);
        }
    }
}