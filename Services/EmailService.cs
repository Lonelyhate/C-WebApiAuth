using apiLeran.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace apiLeran.Services;

public class EmailService : IMessageEmailService
{
    public async Task SendMessage(string email, string subject, string message)
    {
        var emailMessage = new MimeMessage();
        
        emailMessage.From.Add(new MailboxAddress("THC", "nikitafydorov@mail.ru"));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.mail.ru", 25, false);
            await client.AuthenticateAsync("nikitafydorov@mail.ru", "shirokov77");
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}