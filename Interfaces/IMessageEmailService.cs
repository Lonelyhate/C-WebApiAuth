namespace apiLeran.Interfaces;

public interface IMessageEmailService
{
    Task SendMessage(string email, string subject, string message);
}