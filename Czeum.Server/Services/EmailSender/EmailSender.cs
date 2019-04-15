using Czeum.Server.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Server.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private string sendGridKey;

        public EmailSender(IConfiguration config)
        {
            sendGridKey = config.GetValue<string>("SendGrid:SendGridKey");
        }

        public async Task SendConfirmationEmailAsync(string to, string token)
        {
            var client = new SendGridClient(sendGridKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress("czeum@sch.bme.hu", "Czeum Server"),
                Subject = "Verify your email address at Czeum",
                PlainTextContent = "A user has been registered with your e-mail address in our server.\n" +
                    "You can activate your user by pasting this code into the application:\n" +
                    token + "\n If it was not you who registered you can ignore this email."
            };
            msg.AddTo(new EmailAddress(to));
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }
    }
}
