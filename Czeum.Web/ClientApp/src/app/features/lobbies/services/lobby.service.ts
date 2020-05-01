import { Injectable } from '@angular/core';
import {
  GameTypeDto,
  LobbiesClient,
  LobbyDataWrapper,
  LobbyMessagesClient,
  MatchesClient,
  MatchStatus,
  Message
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { LobbyCreateDetails } from '../models/lobby-create.models';
import { RollList } from '../../../shared/models/roll-list';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LobbyService {

  constructor(
    private lobbiesClient: LobbiesClient,
    private matchesClient: MatchesClient,
    private lobbyMessagesClient: LobbyMessagesClient
  ) { }

  getLobbyDetails(id: string): Observable<LobbyDataWrapper> {
    return this.lobbiesClient.getLobby(id);
  }

  getAvailableGameTypes(): Observable<GameTypeDto[]> {
    return this.matchesClient.getGameTypes();
  }

  getCurrentLobby(): Observable<LobbyDataWrapper> {
    return this.lobbiesClient.getCurrentLobby();
  }

  createLobby(details: LobbyCreateDetails): Observable<LobbyDataWrapper> {
    return this.lobbiesClient.createLobby({
      gameIdentifier: details.gameType.identifier,
      lobbyAccess: details.lobbyAccess.lobbyAccess,
      name: details.name
    });
  }

  leaveLobby(): Observable<void> {
    return this.lobbiesClient.leaveLobby();
  }

  getLobbyMessages(lobbyId: string, count = 25, oldestId?: string): Observable<RollList<Message>> {
    return this.lobbyMessagesClient.getMessages(lobbyId, oldestId || '', count).pipe(
      map(res => {
        return new RollList<Message>(res.data, res.hasMoreLeft);
      })
    );
  }

  sendMessage(lobbyId: string, messageText: string): Observable<Message> {
    return this.lobbyMessagesClient.sendMessage(lobbyId, messageText);
  }

  createMatchFromLobby(lobbyId: string): Observable<MatchStatus> {
    return this.matchesClient.createMatch(lobbyId);
  }

  joinLobby(lobbyId: string): Observable<LobbyDataWrapper> {
    return this.lobbiesClient.joinLobby(lobbyId);
  }

  getLobbies(): Observable<LobbyDataWrapper[]> {
    return this.lobbiesClient.getLobbies();
  }

  updateLobby(lobby: LobbyDataWrapper): Observable<LobbyDataWrapper> {
    return this.lobbiesClient.updateLobby(lobby, lobby.content.id);
  }

  inviteUser(lobbyId: string, username: string): Observable<LobbyDataWrapper> {
    return this.lobbiesClient.invitePlayer(lobbyId, username);
  }

  cancelInvite(lobbyId: string, username: string): Observable<LobbyDataWrapper> {
    return this.lobbiesClient.cancelInvitation(lobbyId, username);
  }

}
