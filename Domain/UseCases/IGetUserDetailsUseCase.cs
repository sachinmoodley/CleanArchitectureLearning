using Domain.Interfaces;
using Domain.Requests;

namespace Domain.UseCases
{
    public interface IGetUserDetailsUseCase
    {
        void Execute(GetUserDetailsRequest request, IPresenter presenter);
    }
}