using Domain.Interfaces;
using Domain.Requests;

namespace Domain.UseCases
{
    public interface ICreateUserUseCase
    {
        void Execute(CreateUserRequest request, IPresenter presenter);
    }
}