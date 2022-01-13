using System;
using Domain.Interfaces;
using Domain.Requests;
using Domain.Responses;

namespace Domain.UseCases
{
    public class DeleteUserUseCase : IDeleteUserUseCase
    {
        private readonly IUserGateway _userGateway;

        public DeleteUserUseCase(IUserGateway userGateway)
        {
            _userGateway = userGateway;
        } 

        public void Execute(DeleteUserRequest request, IPresenter presenter)
        {
            try
            {
                var user = _userGateway.Get(Guid.Parse(request.Id));
                if (user == null)
                {
                    presenter.Error("Cannot find user to delete");
                    return;
                }

                _userGateway.Delete(user);
                presenter.Success(new DeleteUserResponse
                {
                    Deleted = true
                });
            }
            catch (Exception e)
            {
                presenter.Error($"Unknown Error Occurred, {e.Message}");
            }
        }
    }
}