using System;
using Domain.Interfaces;
using Domain.Requests;
using Domain.Responses;

namespace Domain.UseCases
{
    public class GetUserDetailsUseCase : IGetUserDetailsUseCase
    {
        private readonly IUserGateway _userGateway;

        public GetUserDetailsUseCase(IUserGateway userGateway)
        {
            _userGateway = userGateway ?? throw new ArgumentNullException(nameof(userGateway));
        }

        public void Execute(GetUserDetailsRequest request, IPresenter presenter)
        {
            try
            {
                var user = _userGateway.Get(Guid.Parse(request.Id));
                if (user == null)
                {
                    presenter.Error("Cannot find user");
                    return;
                }

                presenter.Success(new GetUserDetailsResponse
                {
                    User = user
                });
            }
            catch (Exception e)
            {
                presenter.Error("Unknown error occurred");
                return;
            }
          
        }
    }
}