using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Server.Services.EmailSender
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string to, string uid, string token, string callbackUrl);
        Task SendPasswordResetEmailAsync(string to, string token, string callbackUrl);
    }
}
