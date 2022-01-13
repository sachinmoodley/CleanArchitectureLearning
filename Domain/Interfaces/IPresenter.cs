namespace Domain.Interfaces
{
    public interface IPresenter
    {
        void Success<TResponse>(TResponse response);
        void Error(string error);
    }
}