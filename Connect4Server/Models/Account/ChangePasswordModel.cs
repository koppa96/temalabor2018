using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Models.Account {
	public class ChangePasswordModel {
		public string OldPassword { get; set; }
		public string Password { get; set; }
		public string ConfirmPassword { get; set; }
	}
}
