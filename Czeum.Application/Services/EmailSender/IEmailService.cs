using System;
using System.Threading.Tasks;

namespace Czeum.Application.Services.EmailSender
{
    /// <summary>
    /// Interface for services related to sending emails.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends a e-mail address confirmation e-mail.
        /// </summary>
        /// <param name="to">The address of the receiver</param>
        /// <param name="uid">The id of the user</param>
        /// <param name="token">The activation token</param>
        /// <param name="callbackUrl">The url of the e-mail address activation page</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task SendConfirmationEmailAsync(string to, Guid uid, string token, string callbackUrl);

        /// <summary>
        /// Sends a password reset e-mail.
        /// </summary>
        /// <param name="to">The address of the receiver</param>
        /// <param name="token">The password reset token</param>
        /// <param name="callbackUrl">The url of password reset page</param>
        /// <returns>A task representing the asynchronous operation</returns>
        Task SendPasswordResetEmailAsync(string to, string token, string callbackUrl);
    }
}
