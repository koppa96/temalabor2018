import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from './authentication/services/auth.service';
import { Observable, Subscription } from 'rxjs';
import { AuthState, State } from './reducers';
import { Store } from '@ngrx/store';
import { first, take, takeWhile } from 'rxjs/operators';
import { FriendsService } from './shared/services/friends.service';
import { updateFriendList, addFriend, removeFriend, updateFriend } from './reducers/friend-list/friend-list-actions';
import { FriendDto } from './shared/clients';
import { LobbyService } from './features/lobbies/services/lobby.service';
import { leaveLobby, updateLobby } from './reducers/current-lobby/current-lobby-actions';
import { ObservableHub } from './shared/services/observable-hub.service';
import { toLocalDate } from './shared/services/date-utils';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  interval: any;
  authState: Observable<AuthState>;
  showInitialLoading: boolean;
  subscription = new Subscription();

  constructor(
    private authService: AuthService,
    private observableHub: ObservableHub,
    private friendsService: FriendsService,
    private store: Store<State>,
    private lobbyService: LobbyService
  ) {
    this.authState = this.authService.getAuthState();
  }

  async ngOnInit() {
    this.showInitialLoading = true;
    const isSilentRefreshing = await this.authService.silentRefreshIfRequired();
    if (isSilentRefreshing) {
      this.authState.pipe(
        takeWhile(x => x.isHandling, true)
      ).subscribe(res => {
        if (!res.isHandling) {
          this.showInitialLoading = false;
          if (res.isAuthenticated) {
            this.afterLogin();
          }
        }
      });
    } else {
      this.authState.pipe(
        takeWhile(x => x.isHandling, true)
      ).subscribe(res => {
        this.showInitialLoading = false;
        if (res.isAuthenticated) {
          this.afterLogin();
        }
      });
    }


    const self = this;
    this.interval = setInterval(async () => {
      await self.authService.silentRefreshIfRequired();
    }, 60000);
  }

  async afterLogin() {
    await this.observableHub.createConnection();
    this.friendsService.getFriends().subscribe(res => {
      this.store.dispatch(updateFriendList({ updatedList: res }));
      this.subscription.add(this.observableHub.friendAdded.subscribe(friend => {
        friend.lastDisconnect = toLocalDate(friend.lastDisconnect);
        friend.registrationTime = toLocalDate(friend.lastDisconnect);
        this.store.dispatch(addFriend({ friend }));
      }));
      this.subscription.add(this.observableHub.friendRemoved.subscribe(friendshipId => {
        this.store.dispatch(removeFriend({ friendshipId }));
      }));
      this.subscription.add(this.observableHub.friendConnectionStateChanged.subscribe((friend: FriendDto) => {
        friend.lastDisconnect = toLocalDate(friend.lastDisconnect);
        friend.registrationTime = toLocalDate(friend.lastDisconnect);
        this.store.dispatch(updateFriend({ friend }));
      }));
    });

    this.lobbyService.getCurrentLobby().subscribe(
      result => {
        this.store.dispatch(updateLobby({ newLobby: result }));
        this.registerLobbyCallbacks();
      },
      () => {
        this.store.dispatch(leaveLobby());
        this.registerLobbyCallbacks();
      }
    );
  }

  private registerLobbyCallbacks() {
    this.subscription.add(this.observableHub.lobbyChanged.subscribe(lobby => {
      this.store.select(x => x.currentLobby).pipe(
        take(1)
      ).subscribe(currentLobby => {
        if (currentLobby && lobby.content.id === currentLobby.content.id) {
          this.store.dispatch(updateLobby({ newLobby: lobby }));
        }
      });
    }));

    this.subscription.add(this.observableHub.kickedFromLobby.subscribe(() => this.store.dispatch(leaveLobby())));
  }

  ngOnDestroy() {
    clearInterval(this.interval);
    this.subscription.unsubscribe();
  }
}
