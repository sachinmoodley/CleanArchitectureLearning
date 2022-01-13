using Domain.Interfaces;
using Domain.Requests;

namespace Domain.UseCases
{
    public class ResetPasswordUseCase : IResetPasswordUseCase
    {
        private readonly IUserGateway _userGateway;

        public ResetPasswordUseCase(IUserGateway userGateway)
        {
            _userGateway = userGateway;
        }

        public void Execute(ResetPasswordRequest request, IPresenter presenter)
        {
            var user = _userGateway.GetUserBy(request.Email);
            if (user == null)
            {
                presenter.Error("Cannot find user");
                return;
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                presenter.Error("Passwords do not match");
                return;
            }

            var hasValidOtp =_userGateway.HasValidOtp(user.Id, request.Otp);
            if (!hasValidOtp)
            {
                presenter.Error("Has invalid OTP");
                return;
            }

            _userGateway.UpdatePassword(user.Id, request.NewPassword, request.ConfirmPassword);
        }
    }
}