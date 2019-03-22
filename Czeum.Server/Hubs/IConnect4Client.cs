using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Czeum.Abstractions.DTO;
using Czeum.DTO;

namespace Czeum.Server.Hubs {
	public interface IConnect4Client {
		Task CannotCreateLobbyFromOtherLobby();
		Task LobbyCreated(LobbyData lobby);
		Task LobbyAddedHandler(LobbyData lobby);
		Task MatchCreated(MatchStatus match);
		Task LobbyDeleted(int lobbyId);
		Task NotEnoughPlayersHandler();
		Task CannotSetOtherLobby();
		Task GetInvitationTo(int lobbyId);
		Task GuestDisconnected();
		Task HostDisconnected();
		Task JoinedToLobby(LobbyData lobby);
		Task PlayerJoinedToLobby(string player);
		Task FailedToJoinLobby();
		Task IncorrectMatchIdHandler();
		Task ColumnFullHandler();
		Task MatchFinishedHandler();
		Task NotYourTurnHandler();
		Task SuccessfulPlacement(MatchStatus dto);
		Task SuccessfulEnemyPlacement(MatchStatus dto);
		Task VictoryHandler(MatchStatus dto);
		Task EnemyVictoryHandler(MatchStatus dto);
		Task GuestKicked();
		Task YouHaveBeenKicked();
		Task OnlyHostCanInvite();
		Task NobodyToKick();
		Task LobbyChanged(LobbyData lobby);
		Task UserInvited(LobbyData lobby);
		Task CannotStartOtherMatch();
		Task InvalidLobbyId();
		Task CannotJoinOrCreateWhileQueuing();
		Task CannotQueueWhileInLobby();
		Task InvalidBoardSize();
		Task DrawHandler(MatchStatus dto);
        Task NotYourMatch();
    }
}
