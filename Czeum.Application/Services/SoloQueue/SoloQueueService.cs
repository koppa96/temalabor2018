using System.Collections.Generic;

namespace Czeum.Application.Services.SoloQueue {
	public class SoloQueueService : ISoloQueueService
	{
		private readonly List<string> queuingPlayers;
		private readonly object syncObj;

		public SoloQueueService()
		{
			queuingPlayers = new List<string>();
			syncObj = new object();
		}

		public void JoinSoloQueue(string user)
		{
			lock (syncObj)
			{
				queuingPlayers.Add(user);
			}
		}

		public void LeaveSoloQueue(string user)
		{
			lock (syncObj)
			{
				queuingPlayers.Remove(user);
			}
		}

		public string[] PopFirstTwoPlayers()
		{
			string[] players = new string[2];
			lock (syncObj)
			{
				if (queuingPlayers.Count < 2)
				{
					return null;
				}

				players[0] = queuingPlayers[0];
				players[1] = queuingPlayers[1];
				queuingPlayers.RemoveAt(0);
				queuingPlayers.RemoveAt(0);
			}

			return players;
		}

		public bool IsQueuing(string user)
		{
			lock (syncObj)
			{
				return queuingPlayers.Contains(user);
			}
		}
	}
}
