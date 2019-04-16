using Czeum.Server.Configurations;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Server.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly string _gmailAccount;
        private readonly string _gmailPassword;

        public EmailSender(IConfiguration config)
        {
            _gmailAccount = config.GetValue<string>("Gmail:Username");
            _gmailPassword = config.GetValue<string>("Gmail:Password");
        }

        public async Task SendConfirmationEmailAsync(string to, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Czeum Server", "czeumserver@gmail.com"));
            message.To.Add(new MailboxAddress(to));
            message.Subject = "Confirm your email at Czeum";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<p>An account was registered with your e-mail address. You can activate it with this code:</p>" +
                $"<p>{token}</p>";
            message.Body = bodyBuilder.ToMessageBody();

            await SendMailAsync(message);
        }

        public async Task SendPasswordResetEmailAsync(string to, string token)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Czeum Server", "czeumserver@gmail.com"));
            message.To.Add(new MailboxAddress(to));
            message.Subject = "Password reset at Czeum";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<p>A password reset was requested with your account. You can reset your password with this code:</p>" +
                $"<p>{token}</p>";
            message.Body = bodyBuilder.ToMessageBody();

            await SendMailAsync(message);
        }

        private async Task SendMailAsync(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync(_gmailAccount, _gmailPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
