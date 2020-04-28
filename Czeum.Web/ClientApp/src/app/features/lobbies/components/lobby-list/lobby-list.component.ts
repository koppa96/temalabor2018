import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { GameTypeDto, LobbyDataWrapper, FriendDto } from '../../../../shared/clients';
import { Store } from '@ngrx/store';
import { AuthState, State } from '../../../../reducers';
import { lobbyListOrderings } from '../../models/lobby-list-ordering.models';
import { Ordering } from 'src/app/shared/models/ordering.models';

@Component({
  selector: 'app-lobby-list',
  templateUrl: './lobby-list.component.html',
  styleUrls: ['./lobby-list.component.scss']
})
export class LobbyListComponent implements OnInit, OnChanges, OnDestroy {
  @Input() gameTypes: GameTypeDto[] = [];
  @Input() lobbies: LobbyDataWrapper[] = [];
  @Input() filterEvents: Observable<void>;

  @Output() joinLobby = new EventEmitter<string>();
  @Output() leaveCurrentLobby = new EventEmitter();

  subscription: Subscription;
  currentLobby$: Observable<LobbyDataWrapper>;
  authState$: Observable<AuthState>;
  friendList$: Observable<FriendDto[]>;
  filteredLobbies: LobbyDataWrapper[] = [];
  filterText = '';
  orderings = lobbyListOrderings;
  selectedOrdering: Ordering<LobbyDataWrapper>;

  constructor(store: Store<State>) {
    this.selectedOrdering = this.orderings[0];
    console.log(this.orderings);
    this.currentLobby$ = store.select(x => x.currentLobby);
    this.authState$ = store.select(x => x.authState);
    this.friendList$ = store.select(x => x.friendList);
  }

  ngOnInit() {
    this.subscription = this.filterEvents.subscribe(() => {
      this.filterAndSortLobbies();
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.lobbies) {
      this.filterAndSortLobbies();
    }
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  filterAndSortLobbies() {
    this.filteredLobbies = this.lobbies.filter(
      x => !x.content.name && `${x.content.host} szobája`.toUpperCase().includes(this.filterText.toUpperCase()) ||
        x.content.name && x.content.name.toUpperCase().includes(this.filterText.toUpperCase())
    ).sort(this.selectedOrdering.comparator);
  }

}
