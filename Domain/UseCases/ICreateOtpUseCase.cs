using Domain.Interfaces;
using Domain.Requests;

namespace Domain.UseCases
{
    public interface ICreateOtpUseCase
    {
        void Execute(CreateOtpRequest request, IPresenter presenter);
    }
}