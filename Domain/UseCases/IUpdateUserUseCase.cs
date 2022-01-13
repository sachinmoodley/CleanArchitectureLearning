using Domain.Interfaces;
using Domain.Requests;

namespace Domain.UseCases
{
    public interface IUpdateUserUseCase
    {
        void Execute(UpdateUserRequest request, IPresenter presenter);
    }
}