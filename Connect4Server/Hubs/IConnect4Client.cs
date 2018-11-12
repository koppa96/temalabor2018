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
		Task LobbySettingsChanged(LobbyData lobby);
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
		Task SuccessfulPlacement(int column);
		Task SuccessfulEnemyPlacement(int column);
		Task VictoryHandler(int column);
		Task EnemyVictoryHandler(int column);
		Task GuestKicked();
		Task YouHaveBeenKicked();
	}
}
