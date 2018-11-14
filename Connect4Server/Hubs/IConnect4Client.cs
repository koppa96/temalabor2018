using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Connect4Dtos;

namespace Connect4Server.Hubs {
	public interface IConnect4Client {
		Task CannotCreateLobbyFromOtherLobby();
		Task LobbyCreated(LobbyData lobby);
		Task LobbyAddedHandler(LobbyData lobby);
		Task MatchCreated(MatchDto match);
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
		Task SuccessfulPlacement(int matchId, int column);
		Task SuccessfulEnemyPlacement(int matchId, int column);
		Task VictoryHandler(int matchId, int column);
		Task EnemyVictoryHandler(int matchId, int column);
		Task GuestKicked();
		Task YouHaveBeenKicked();
		Task OnlyHostCanInvite();
		Task NobodyToKick();
		Task LobbyChanged(LobbyData lobby);
		Task UserInvited(LobbyData lobby);
		Task CannotStartOtherMatch();
		Task InvalidLobbyId();
	}
}
