import { Component, OnInit } from '@angular/core';
import { Notification, mockNotifications } from '../../models/notification.models';
import { Store } from '@ngrx/store';
import { State } from '../../../reducers';
import { Observable } from 'rxjs';
import { leaveSoloQueue } from '../../../reducers/solo-queue/solo-queue-actions';
import { map } from 'rxjs/operators';
import { NotificationType } from '../../clients';
import { Router } from '@angular/router';
import { FriendsService } from '../../services/friends.service';
import { addFriend } from '../../../reducers/friend-list/friend-list-actions';
import { LobbyService } from '../../../features/lobbies/services/lobby.service';
import { updateLobby } from '../../../reducers/current-lobby/current-lobby-actions';
import { deleteNotification } from '../../../reducers/notifications/notifications-actions';
import { NotificationService } from '../../services/notification.service';
import { removeIncomingFriendRequest } from '../../../reducers/friend-list/incoming-friend-requests-actions';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent implements OnInit {
  isQueuing: Observable<boolean>;
  notifications: Notification[] = mockNotifications;
  notifications$: Observable<Notification[]>;

  queueNotification: Notification = new Notification({
    id: '',
    icon: 'speed',
    text: 'Gyors játék keresés folyamatban...',
    actions: {
      primary: {
        name: 'LEÁLLÍTÁS',
        action: () => this.store.dispatch(leaveSoloQueue())
      }
    }
  });

  constructor(
    private store: Store<State>,
    private router: Router,
    private friendsService: FriendsService,
    private lobbyService: LobbyService,
    private notificationService: NotificationService
  ) {
    this.isQueuing = this.store.select(x => x.isQueueing);
    this.notifications$ = this.store.select(x => x.notifications).pipe(
      map(dto => {
        return dto.map(x => {
          switch (x.type) {
            case NotificationType.AchivementUnlocked:
              return new Notification({
                id: x.id,
                icon: 'emoji_events',
                text: 'Mérföldkő feloldva!',
                actions: {
                  primary: {
                    name: 'MEGTEKINTÉS',
                    action: () => { this.router.navigate(['/home']); }
                  }
                }
              });
            case NotificationType.FriendRequestAccepted:
              return new Notification({
                id: x.id,
                icon: 'group',
                text: `${x.senderUserName} elfogadta a barátkérelmed!`
              });
            case NotificationType.FriendRequestReceived:
              return new Notification({
                id: x.id,
                icon: 'group_add',
                text: `${x.senderUserName} barátkérelmet küldött!`,
                data: x.data,
                actions: {
                  primary: {
                    name: 'ELFOGADÁS',
                    action: (requestId: string) => {
                      this.friendsService.acceptRequest(requestId).subscribe(result => {
                        this.store.dispatch(addFriend({ friend: result }));
                        this.store.dispatch(removeIncomingFriendRequest({ requestId }));
                        this.notificationService.deleteNotification(x.id).subscribe(() => {
                          this.store.dispatch(deleteNotification({ id: x.id }));
                        });
                      });
                    }
                  },
                  secondary: {
                    name: 'ELUTASÍTÁS',
                    action: (requestId: string) => {
                      this.friendsService.rejectRequest(requestId).subscribe(() => {
                        this.store.dispatch(removeIncomingFriendRequest({ requestId }));
                        this.notificationService.deleteNotification(x.id).subscribe(() => {
                          this.store.dispatch(deleteNotification({ id: x.id }));
                        });
                      });
                    }
                  }
                }
              });
            case NotificationType.InviteReceived:
              return new Notification({
                id: x.id,
                icon: 'mail',
                text: `${x.senderUserName} meghívott, hogy csatlakozz a szobájába!`,
                data: x.data,
                actions: {
                  primary: {
                    name: 'ELFOGADÁS',
                    action: (lobbyId: string) => {
                      this.lobbyService.joinLobby(lobbyId).subscribe(lobby => {
                        this.store.dispatch(updateLobby({ newLobby: lobby }));
                      });
                    }
                  }
                }
              });
            case NotificationType.Poked:
              return new Notification({
                id: x.id,
                icon: 'warning',
                text: `${x.senderUserName} megbökött, mert régóta nem léptél!`
              });
          }
        });
      })
    );
  }

  ngOnInit() {
  }

  deleteRequested(notification: Notification) {
    this.notificationService.deleteNotification(notification.id).subscribe(() => {
      this.store.dispatch(deleteNotification({ id: notification.id }));
    });
  }

}
