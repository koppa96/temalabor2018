import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from './authentication/services/auth.service';
import { Observable } from 'rxjs';
import { AuthState, State } from './reducers';
import { Store } from '@ngrx/store';
import { takeWhile } from 'rxjs/operators';
import { HubService } from './shared/services/hub.service';
import { FriendsService } from './shared/services/friends.service';
import { updateFriendList, addFriend, removeFriend, updateFriend } from './reducers/friend-list/friend-list-actions';
import { FriendDto } from './shared/clients';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  interval: any;
  authState: Observable<AuthState>;
  showInitialLoading: boolean;

  constructor(
    private authService: AuthService,
    private hubService: HubService,
    private friendsService: FriendsService,
    private store: Store<State>
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

  afterLogin() {
    this.hubService.connect();
    this.friendsService.getFriends().subscribe(res => {
      this.store.dispatch(updateFriendList({ updatedList: res }));
      this.hubService.registerCallback('FriendAdded', (friend: FriendDto) => {
        this.store.dispatch(addFriend({ friend }));
      });
      this.hubService.registerCallback('FriendRemoved', (friendshipId: string) => {
        this.store.dispatch(removeFriend({ friendshipId }));
      });
      this.hubService.registerCallback('FriendConnectionStateChanged', (friend: FriendDto) => {
        this.store.dispatch(updateFriend({ friend }));
      });
    });
  }

  async ngOnDestroy() {
    clearInterval(this.interval);
    this.hubService.removeCallback('FriendAdded');
    this.hubService.removeCallback('FriendRemoved');
    this.hubService.removeCallback('FriendConnectionStateChanged');
    await this.hubService.disconnect();
  }
}
