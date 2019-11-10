namespace Czeum.Core.DTOs.UserManagement {
    /// <summary>
    /// The data used by the AccountController to update the user's password.
    /// </summary>
	public class ChangePasswordModel {
		public string OldPassword { get; set; }
		public string Password { get; set; }
    }
}
