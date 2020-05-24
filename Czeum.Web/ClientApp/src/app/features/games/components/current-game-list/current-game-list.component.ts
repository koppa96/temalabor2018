import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { GameState, GameTypeDto, MatchStatus } from '../../../../shared/clients';
import { RollList } from '../../../../shared/models/roll-list';
import { Ordering } from '../../../../shared/models/ordering.models';
import { gameListOrderings } from '../../models/game-list-orderings';
import { Store } from '@ngrx/store';
import { AuthState, State } from '../../../../reducers';
import { Observable, Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-current-game-list',
  templateUrl: './current-game-list.component.html',
  styleUrls: ['./current-game-list.component.scss']
})
export class CurrentGameListComponent implements OnInit, OnChanges, OnDestroy {
  @Input() matches: RollList<MatchStatus> = new RollList<MatchStatus>();
  @Input() gameTypes: GameTypeDto[] = [];
  @Input() matchListUpdated: Observable<void>;
  @Output() loadMore = new EventEmitter();

  filterText = '';
  filteredMatches: MatchStatus[];
  selectedOrdering: Ordering<MatchStatus>;
  orderings = gameListOrderings;
  authState$: Observable<AuthState>;

  subscription: Subscription;

  constructor(private store: Store<State>, private router: Router) {
    this.authState$ = store.select(x => x.authState);
  }

  ngOnInit() {
    this.selectedOrdering = this.orderings[0];
    this.filterAndSortMatches();
    this.subscription = this.matchListUpdated.subscribe(() => this.filterAndSortMatches());
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.matches) {
      this.filterAndSortMatches();
    }
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  filterAndSortMatches() {
    if (this.selectedOrdering) {
      this.filteredMatches = this.matches.elements
        .filter(x => x.players.some(p => p.username.toLowerCase().includes(this.filterText.toLowerCase())))
        .sort(this.selectedOrdering.comparator);
    }
  }

  onPlayClicked(matchStatus: MatchStatus) {
    this.router.navigate([ `/games/${matchStatus.id}/${matchStatus.currentBoard.gameIdentifier}` ]);
  }

}
