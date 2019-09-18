using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Czeum.Application.Services.EmailSender
{
    public class EmailService : IEmailService
    {
        private readonly string gmailAccount;
        private readonly string gmailPassword;

        public EmailService(IConfiguration config)
        {
            gmailAccount = config.GetValue<string>("Gmail:Username");
            gmailPassword = config.GetValue<string>("Gmail:Password");
        }

        public async Task SendConfirmationEmailAsync(string to, string uid, string token, string callbackUrl)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Czeum Server", "czeumserver@gmail.com"));
            message.To.Add(new MailboxAddress(to));
            message.Subject = "Confirm your email at Czeum";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<p>An account was registered with your e-mail address. You can activate it with this code:</p>" +
                $"<p>{token}</p>" +
                $"<p>You can also click this link to activate your account: <a href='{callbackUrl}'>{callbackUrl}</a></p>";
            message.Body = bodyBuilder.ToMessageBody();

            await SendMailAsync(message);
        }

        public async Task SendPasswordResetEmailAsync(string to, string token, string callbackUrl)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Czeum Server", "czeumserver@gmail.com"));
            message.To.Add(new MailboxAddress(to));
            message.Subject = "Password reset at Czeum";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<p>A password reset was requested with your account. You can reset your password with this code:</p>" +
                $"<p>{token}</p>" +
                $"<p>You can also click this link to activate your account: <a href='{callbackUrl}'>{callbackUrl}</a></p>" ;
            message.Body = bodyBuilder.ToMessageBody();

            await SendMailAsync(message);
        }

        private async Task SendMailAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(gmailAccount, gmailPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Czeum Server", "czeumserver@gmail.com"));
            message.To.Add(new MailboxAddress(email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
            message.Body = bodyBuilder.ToMessageBody();

            await SendMailAsync(message);
        }
    }
}
