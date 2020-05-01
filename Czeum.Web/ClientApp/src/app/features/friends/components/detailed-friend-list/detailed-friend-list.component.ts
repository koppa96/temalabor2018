import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from 'src/app/reducers';
import { Observable, Subscription } from 'rxjs';
import { FriendDto, LobbyDataWrapper } from 'src/app/shared/clients';
import { detailedFriendListOrderings } from '../../models/ordering.models';
import { Ordering } from '../../../../shared/models/ordering.models';
import { getLastOnlineText } from '../../../../shared/services/date-utils';
import { MatDialog } from '@angular/material';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogResult } from '../../../../shared/models/dialog.models';
import { FriendsService } from '../../../../shared/services/friends.service';
import { updateFriendList } from '../../../../reducers/friend-list/friend-list-actions';
import { map, take } from 'rxjs/operators';
import { LobbyService } from '../../../lobbies/services/lobby.service';
import { updateLobby } from '../../../../reducers/current-lobby/current-lobby-actions';

@Component({
  selector: 'app-detailed-friend-list',
  templateUrl: './detailed-friend-list.component.html',
  styleUrls: ['./detailed-friend-list.component.scss']
})
export class DetailedFriendListComponent implements OnInit, OnDestroy {
  filteredFriendList: FriendDto[] = [];
  friendList: FriendDto[] = [];
  filterText = '';

  orderings = detailedFriendListOrderings;
  selectedOrdering: Ordering<FriendDto>;

  subscription: Subscription;
  currentLobby$: Observable<LobbyDataWrapper>;

  constructor(
    private store: Store<State>,
    private dialog: MatDialog,
    private friendsService: FriendsService,
    private lobbyService: LobbyService
  ) {
    this.currentLobby$ = this.store.select(x => x.currentLobby);
  }

  ngOnInit() {
    this.selectedOrdering = this.orderings[0];
    this.subscription = this.store.select(x => x.friendList).subscribe(result => {
      this.friendList = result;
      this.filterAndSortFriends();
    });
  }

  filterAndSortFriends() {
    this.filteredFriendList = this.friendList.filter(x => x.username.toLowerCase().includes(this.filterText.toLowerCase()))
      .sort(this.selectedOrdering.comparator);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  getLastOnlineText(friend: FriendDto): string {
    return getLastOnlineText(friend.lastDisconnect);
  }

  isInvited(friend: FriendDto): Observable<boolean> {
    return this.currentLobby$.pipe(
      map(currentLobby => currentLobby && currentLobby.content.invitedPlayers.some(x => x === friend.username))
    );
  }

  canInvite(friend: FriendDto): Observable<boolean> {
    return this.currentLobby$.pipe(
      map(currentLobby => currentLobby && !currentLobby.content.invitedPlayers.some(x => x === friend.username) &&
        currentLobby.content.host !== friend.username && !currentLobby.content.guests.some(x => x === friend.username))
    );
  }

  onInvite(friend: FriendDto) {
    this.currentLobby$.pipe(
      take(1)
    ).subscribe(currentLobby => {
      this.lobbyService.inviteUser(currentLobby.content.id, friend.username).subscribe(lobby => {
        this.store.dispatch(updateLobby({ newLobby: lobby }));
      });
    });
  }

  onInviteRevoked(friend: FriendDto) {
    this.currentLobby$.pipe(
      take(1)
    ).subscribe(currentLobby => {
      this.lobbyService.cancelInvite(currentLobby.content.id, friend.username).subscribe(lobby => {
        this.store.dispatch(updateLobby({ newLobby: lobby }));
      });
    });
  }

  onMessage(friend: FriendDto) {
    // TODO: Redirect to messaging page
  }

  onDelete(friend: FriendDto) {
    this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Megerősítés szükséges',
        text: `Biztosan törölni szeretnéd ${friend.username} felhasználót a barátaid közül?`,
        autoFocus: false
      }
    }).afterClosed().subscribe((result: ConfirmDialogResult) => {
      if (result && result.shouldProceed) {
        this.friendsService.removeFriend(friend.friendshipId).subscribe(() => {
          const newFriendList = this.friendList.filter(x => x.friendshipId !== friend.friendshipId);
          this.store.dispatch(updateFriendList({ updatedList: newFriendList }));
        });
      }
    });
  }

}
