import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { RollList } from '../../../../shared/models/roll-list';
import { GameTypeDto, MatchStatus } from '../../../../shared/clients';
import { Observable, Subscription } from 'rxjs';
import { Ordering } from '../../../../shared/models/ordering.models';
import { gameListOrderings } from '../../models/game-list-orderings';
import { AuthState, State } from '../../../../reducers';
import { Store } from '@ngrx/store';
import { Router } from '@angular/router';

@Component({
  selector: 'app-previous-game-list',
  templateUrl: './previous-game-list.component.html',
  styleUrls: ['./previous-game-list.component.scss']
})
export class PreviousGameListComponent implements OnInit, OnChanges, OnDestroy {
  @Input() matches: RollList<MatchStatus> = new RollList<MatchStatus>();
  @Input() gameTypes: GameTypeDto[] = [];
  @Input() matchListUpdated: Observable<void>;
  @Output() loadMore = new EventEmitter();

  filterText = '';
  filteredMatches: MatchStatus[];
  selectedOrdering: Ordering<MatchStatus>;
  orderings = gameListOrderings;
  authState$: Observable<AuthState>;

  subscription = new Subscription();

  constructor(
    private store: Store<State>,
    private router: Router
  ) {
    this.authState$ = this.store.select(x => x.authState);
  }

  ngOnInit() {
    this.selectedOrdering = this.orderings[0];
    this.filterAndSortMatches();
    this.subscription.add(this.matchListUpdated.subscribe(() => this.filterAndSortMatches()));
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes.matches) {
      console.log(this.matches);
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
