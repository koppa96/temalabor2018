import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { LobbyDataWrapper } from '../../../../shared/clients';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { HubService } from '../../../../shared/services/hub.service';
import { LobbyService } from '../../services/lobby.service';
import { take } from 'rxjs/operators';
import { leaveLobby, updateLobby } from '../../../../reducers/current-lobby/current-lobby-actions';

@Component({
  templateUrl: './lobby-details.page.component.html',
  styleUrls: ['./lobby-details.page.component.scss']
})
export class LobbyDetailsPageComponent implements OnInit, OnDestroy {
  currentLobby$: Observable<LobbyDataWrapper>;

  constructor(
    private store: Store<State>,
    private hubService: HubService,
    private lobbyService: LobbyService
  ) {
    this.currentLobby$ = store.select(x => x.currentLobby);
  }

  ngOnInit() {
    this.currentLobby$.pipe(
      take(1)
    ).subscribe(res => {
      if (res === null) {
        return;
      } else {
        this.lobbyService.getLobbyDetails(res.content.id).subscribe(lobbyRes => {
          this.store.dispatch(updateLobby({ newLobby: lobbyRes }));
          this.hubService.registerCallback('LobbyChanged', this.onLobbyChange);
          this.hubService.registerCallback('KickedFromLobby', this.onKicked);
        });
      }
    });
  }

  ngOnDestroy() {
    this.hubService.removeCallback('LobbyChanged', this.onLobbyChange);
    this.hubService.removeCallback('KickedFromLobby', this.onKicked);
  }

  onLobbyChange(lobbyData: LobbyDataWrapper) {
    this.currentLobby$.pipe(
      take(1)
    ).subscribe(res => {
      if (lobbyData.content.id === res.content.id) {
        this.store.dispatch(updateLobby({ newLobby: lobbyData }));
      }
    });
  }

  onKicked(lobbyData: LobbyDataWrapper) {
    this.store.dispatch(leaveLobby());
  }

}
