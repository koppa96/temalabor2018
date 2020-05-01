import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from '../../../reducers';
import { Observable } from 'rxjs';
import { FriendDto, LobbyDataWrapper } from '../../clients';
import { LobbyService } from '../../../features/lobbies/services/lobby.service';
import { take } from 'rxjs/operators';
import { updateLobby } from '../../../reducers/current-lobby/current-lobby-actions';

@Component({
  selector: 'app-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.scss']
})
export class FriendsListComponent implements OnInit {
  friendList$: Observable<FriendDto[]>;
  currentLobby$: Observable<LobbyDataWrapper>;

  constructor(
    private store: Store<State>,
    private lobbyService: LobbyService
  ) { }

  ngOnInit() {
    this.currentLobby$ = this.store.select(x => x.currentLobby);
    this.friendList$ = this.store.select(s => s.friendList);
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

  onCancelInvite(friend: FriendDto) {
    this.currentLobby$.pipe(
      take(1)
    ).subscribe(currentLobby => {
      this.lobbyService.cancelInvite(currentLobby.content.id, friend.username).subscribe(lobby => {
        this.store.dispatch(updateLobby({ newLobby: lobby }));
      });
    });
  }
}
