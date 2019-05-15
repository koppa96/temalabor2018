namespace Czeum.DTO.UserManagement {
    /// <summary>
    /// The data that's used by the AccountController to register users.
    /// </summary>
    public class RegisterModel {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
