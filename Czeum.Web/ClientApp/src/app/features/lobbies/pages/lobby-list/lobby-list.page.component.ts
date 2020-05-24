import { Component, OnDestroy, OnInit } from '@angular/core';
import { LobbyService } from '../../services/lobby.service';
import { GameTypeDto, LobbyDataWrapper, NotificationType } from '../../../../shared/clients';
import { Router } from '@angular/router';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { leaveLobby } from '../../../../reducers/current-lobby/current-lobby-actions';
import { MatSnackBar } from '@angular/material';
import { Subject, Subscription } from 'rxjs';
import { ObservableHub } from '../../../../shared/services/observable-hub.service';
import { NotificationService } from '../../../../shared/services/notification.service';
import { take } from 'rxjs/operators';
import { deleteNotification } from '../../../../reducers/notifications/notifications-actions';
import { not } from 'rxjs/internal-compatibility';

@Component({
  templateUrl: './lobby-list.page.component.html',
  styleUrls: ['./lobby-list.page.component.scss']
})
export class LobbyListPageComponent implements OnInit, OnDestroy {
  gameTypes: GameTypeDto[] = [];
  lobbies: LobbyDataWrapper[] = [];
  filterEvent = new Subject<void>();

  subscriptions: Subscription[] = [];

  constructor(
    private lobbyService: LobbyService,
    private router: Router,
    private store: Store<State>,
    private snackBar: MatSnackBar,
    private observableHub: ObservableHub,
    private notificationService: NotificationService
  ) { }

  ngOnInit() {
    this.lobbyService.getAvailableGameTypes().subscribe(res => {
      this.gameTypes = res;
    });

    this.lobbyService.getLobbies().subscribe(res => {
      this.lobbies = res;

      this.subscriptions.push(this.observableHub.lobbyAdded.subscribe(lobby => {
        this.lobbies.splice(0, 0, lobby);
        this.filterEvent.next();
      }));

      this.subscriptions.push(this.observableHub.lobbyDeleted.subscribe(lobbyId => {
        const lobbyIndex = this.lobbies.findIndex(x => x.content.id === lobbyId);
        if (lobbyIndex !== -1) {
          this.lobbies.splice(lobbyIndex, 1);
          this.filterEvent.next();
        }
      }));

      this.subscriptions.push(this.observableHub.lobbyChanged.subscribe(lobby => {
        const lobbyIndex = this.lobbies.findIndex(x => x.content.id === lobby.content.id);
        if (lobbyIndex !== -1) {
          this.lobbies.splice(lobbyIndex, 1, lobby);
          this.filterEvent.next();
        }
      }));
    });
  }

  ngOnDestroy() {
    for (const subscription of this.subscriptions) {
      subscription.unsubscribe();
    }
  }

  joinLobby(lobbyId: string) {
    this.lobbyService.joinLobby(lobbyId).subscribe(
      () => {
        this.store.select(x => x.notifications).pipe(
          take(1)
        ).subscribe(notifications => {
          const notification = notifications.find(x => x.type === NotificationType.InviteReceived && x.data === lobbyId);
          if (notification) {
            this.notificationService.deleteNotification(notification.id).subscribe(() => {
              this.store.dispatch(deleteNotification({ id: notification.id }));
            });
          }
        });
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
