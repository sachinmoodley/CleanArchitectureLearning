namespace Domain.Interfaces
{
    public interface IMessageGateway
    {
        void SendMessage(int otp);
    }
}