using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Server.Services {
	public class SoloQueueService : ISoloQueueService
	{
		private readonly List<string> _queuingPlayers;
		private readonly object _syncObj;

		public SoloQueueService()
		{
			_queuingPlayers = new List<string>();
			_syncObj = new object();
		}

		public void JoinSoloQueue(string user)
		{
			lock (_syncObj)
			{
				_queuingPlayers.Add(user);
			}
		}

		public void LeaveSoloQueue(string user)
		{
			lock (_syncObj)
			{
				_queuingPlayers.Remove(user);
			}
		}

		public string[] PopFirstTwoPlayers()
		{
			string[] players = new string[2];
			lock (_syncObj)
			{
				if (_queuingPlayers.Count < 2)
				{
					return null;
				}

				players[0] = _queuingPlayers[0];
				players[1] = _queuingPlayers[1];
				_queuingPlayers.RemoveAt(0);
				_queuingPlayers.RemoveAt(0);
			}

			return players;
		}

		public bool IsQueuing(string user)
		{
			lock (_syncObj)
			{
				return _queuingPlayers.Contains(user);
			}
		}
	}
}
