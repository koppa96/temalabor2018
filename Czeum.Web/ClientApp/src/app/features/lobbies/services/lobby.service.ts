import { Injectable } from '@angular/core';
import { GameTypeDto, LobbiesClient, LobbyDataWrapper, MatchesClient } from '../../../shared/clients';
import { Observable } from 'rxjs';
import { LobbyCreateDetails } from '../models/lobby-create.models';

@Injectable()
export class LobbyService {

  constructor(private lobbiesClient: LobbiesClient, private matchesClient: MatchesClient) { }

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

}
