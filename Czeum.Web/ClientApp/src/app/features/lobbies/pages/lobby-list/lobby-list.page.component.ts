import { Component, OnDestroy, OnInit } from '@angular/core';
import { LobbyService } from '../../services/lobby.service';
import { GameTypeDto, LobbyDataWrapper } from '../../../../shared/clients';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { leaveLobby } from '../../../../reducers/current-lobby/current-lobby-actions';
import { HubService } from '../../../../shared/services/hub.service';
import { MatSnackBar } from '@angular/material';
import { Subject } from 'rxjs';

@Component({
  templateUrl: './lobby-list.page.component.html',
  styleUrls: ['./lobby-list.page.component.scss']
})
export class LobbyListPageComponent implements OnInit, OnDestroy {
  gameTypes: GameTypeDto[] = [];
  lobbies: LobbyDataWrapper[] = [];
  filterEvent = new Subject<void>();

  constructor(
    private lobbyService: LobbyService,
    private router: Router,
    private store: Store<State>,
    private hubService: HubService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {
    this.lobbyService.getAvailableGameTypes().subscribe(res => {
      this.gameTypes = res;
    });

    this.lobbyService.getLobbies().subscribe(res => {
      this.lobbies = res;
      console.log(this.lobbies);
      this.hubService.registerCallback('LobbyAdded', (lobby: LobbyDataWrapper) => {
        this.lobbies.splice(0, 0, lobby);
        this.filterEvent.next();
      });

      this.hubService.registerCallback('LobbyDeleted', (lobbyId: string) => {
        const lobbyIndex = this.lobbies.findIndex(x => x.content.id === lobbyId);
        if (lobbyIndex !== -1) {
          this.lobbies.splice(lobbyIndex, 1);
          this.filterEvent.next();
        }
      });

      this.hubService.registerCallback('LobbyChanged', (lobby: LobbyDataWrapper) => {
        const lobbyIndex = this.lobbies.findIndex(x => x.content.id === lobby.content.id);
        if (lobbyIndex !== -1) {
          this.lobbies.splice(lobbyIndex, 1, lobby);
          this.filterEvent.next();
        }
      });
    });
  }

  ngOnDestroy() {
    this.hubService.removeCallback('LobbyAdded');
    this.hubService.removeCallback('LobbyDeleted');
    this.hubService.removeCallback('LobbyChanged');
  }

  joinLobby(lobbyId: string) {
    this.lobbyService.joinLobby(lobbyId).subscribe(
      () => {
        this.router.navigate(['/lobbies/mine']);
      },
      () => {
        this.snackBar.open(
          'Nem csatlakozhatsz amíg szobában vagy, vagy gyorsmeccsre vársz!',
          'BEZÁR',
          {
            duration: 5000,
            panelClass: [ 'snackbar' ]
          });
      });
  }

  leaveCurrentLobby() {
    this.lobbyService.leaveLobby().subscribe(() => {
      this.store.dispatch(leaveLobby());
    });
  }
}
