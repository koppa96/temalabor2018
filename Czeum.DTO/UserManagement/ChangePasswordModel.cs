namespace Czeum.DTO.UserManagement {
	public class ChangePasswordModel {
		public string OldPassword { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
	}
}
