using Domain.Interfaces;
using Domain.Requests;

namespace Domain.UseCases
{
    public interface IDeleteUserUseCase
    {
        void Execute(DeleteUserRequest request, IPresenter presenter);
    }
}