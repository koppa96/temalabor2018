using Connect4Server.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Connect4Server.Services {
	public class SoloQueueService {
		public List<string> QueingPlayers;

		public SoloQueueService() {
			QueingPlayers = new List<string>();
		}
	}
}
