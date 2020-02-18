using Czeum.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Czeum.Core.ClientCallbacks
{
    public interface IMessageClient
    {
        Task ReceiveDirectMessage(Guid friendshipId, Message message);
        Task ReceiveMatchMessage(Guid matchId, Message message);
        Task ReceiveLobbyMessage(Guid lobbyId, Message message);
    }
}
