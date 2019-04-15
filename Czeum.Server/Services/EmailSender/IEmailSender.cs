using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Server.Services.EmailSender
{
    public interface IEmailSender
    {
        Task SendConfirmationEmailAsync(string to, string token);
    }
}
