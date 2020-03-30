using System;
using System.Threading.Tasks;
using Czeum.Core.Services;
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

        public async Task SendConfirmationEmailAsync(string to, string callbackUrl)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Czeum Server", gmailAccount));
            message.To.Add(new MailboxAddress(to));
            message.Subject = "Czeum - E-mail megerősítés";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<p>Egy fiók lett létrehozva ezzel az e-mail címmel. Használd az alábbi linket a regisztráció aktiválásához!</p>" +
                $"<p><a href='{callbackUrl}'>Aktiválás</a></p>";
            message.Body = bodyBuilder.ToMessageBody();

            await SendMailAsync(message);
        }

        public async Task SendPasswordResetEmailAsync(string to, string callbackUrl)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Czeum Server", "server.czeum@gmail.com"));
            message.To.Add(new MailboxAddress(to));
            message.Subject = "Czeum - Jelszó visszaállítás";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = "<p>Az erre az e-mail címre regisztrált fiókodra jelszóvisszaállítási kérés érkezett. Használd az alábbi linket a jelszó visszaállításához!</p>" +
                $"<p><a href='{callbackUrl}'>Jelszó visszaállítása</a></p>" ;
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
            message.From.Add(new MailboxAddress("Czeum Server", "server.czeum@gmail.com"));
            message.To.Add(new MailboxAddress(email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
            message.Body = bodyBuilder.ToMessageBody();

            await SendMailAsync(message);
        }
    }
}
