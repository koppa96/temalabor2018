import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { GameTypeDto, LobbyAccess, LobbyDataWrapper } from '../../../../shared/clients';
import { Store } from '@ngrx/store';
import { AuthState, State } from '../../../../reducers';
import { FriendListItem } from '../../../../shared/models/friend-list.models';

@Component({
  selector: 'app-lobby-list',
  templateUrl: './lobby-list.component.html',
  styleUrls: ['./lobby-list.component.scss']
})
export class LobbyListComponent implements OnInit {
  @Input() gameTypes: GameTypeDto[] = [];
  @Input() lobbies: LobbyDataWrapper[] = [];

  @Output() joinLobby = new EventEmitter<string>();

  currentLobby$: Observable<LobbyDataWrapper>;
  authState$: Observable<AuthState>;
  friendList$: Observable<FriendListItem[]>;

  constructor(store: Store<State>) {
    this.currentLobby$ = store.select(x => x.currentLobby);
    this.authState$ = store.select(x => x.authState);
    this.friendList$ = store.select(x => x.friendList);
  }

  ngOnInit() {
  }

}
