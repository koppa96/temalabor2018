using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Services {
	public class CustomUserIdProvider : IUserIdProvider {
		public string GetUserId(HubConnectionContext connection) {
			return connection.User?.Identity.Name;
		}
	}
}
