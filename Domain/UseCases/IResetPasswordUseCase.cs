using Domain.Interfaces;
using Domain.Requests;

namespace Domain.UseCases
{
    public interface IResetPasswordUseCase
    {
        void Execute(ResetPasswordRequest request, IPresenter presenter);
    }
}