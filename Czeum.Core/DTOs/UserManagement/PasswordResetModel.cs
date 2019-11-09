namespace Czeum.Core.DTOs.UserManagement
{
    /// <summary>
    /// The data used by the AccountController to reset the user's password.
    /// </summary>
    public class PasswordResetModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
