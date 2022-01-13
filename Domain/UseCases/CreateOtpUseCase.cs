using System;
using Domain.Interfaces;
using Domain.Requests;
using Domain.Responses;

namespace Domain.UseCases
{
    public class CreateOtpUseCase : ICreateOtpUseCase
    {
        private readonly IUserGateway _userGateway;
        private readonly IMessageGateway _messageGateway;

        public CreateOtpUseCase(IUserGateway userGateway, IMessageGateway messageGateway)
        {
            _userGateway = userGateway ?? throw new ArgumentNullException(nameof(userGateway));
            _messageGateway = messageGateway ?? throw new ArgumentNullException(nameof(messageGateway));
        }

        public void Execute(CreateOtpRequest request, IPresenter presenter)
        {
            var user = _userGateway.GetUserBy(request.Email);
            if (user == null)
            {
                Console.WriteLine("Invalid email address");
                return;
            }

            var random = new Random();
            var otp = random.Next(0, 99999);
            _userGateway.StoreOtp(user.Id, otp);

            _messageGateway.SendMessage(otp);

            presenter.Success(new CreateOtpResponse
            {
                Otp = otp
            });
        }
    }
}