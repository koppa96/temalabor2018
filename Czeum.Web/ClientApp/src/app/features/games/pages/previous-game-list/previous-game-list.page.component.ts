import { Component, OnDestroy, OnInit } from '@angular/core';
import { RollList } from '../../../../shared/models/roll-list';
import { GameState, GameTypeDto, MatchStatus } from '../../../../shared/clients';
import { MatchService } from '../../services/match.service';
import { Subject, Subscription } from 'rxjs';
import { ObservableHub } from '../../../../shared/services/observable-hub.service';

@Component({
  selector: 'app-current-game-list.page',
  templateUrl: './previous-game-list.page.component.html',
  styleUrls: ['./previous-game-list.page.component.scss']
})
export class PreviousGameListPageComponent implements OnInit, OnDestroy {
  matches: RollList<MatchStatus> = new RollList<MatchStatus>();
  gameTypes: GameTypeDto[] = [];
  matchListUpdated = new Subject();

  subscription = new Subscription();

  constructor(
    private matchService: MatchService,
    private observableHub: ObservableHub
  ) { }

  ngOnInit() {
    this.matchService.getAvailableGameTypes().subscribe(res => {
      this.gameTypes = res;
    });

    this.matchService.getPreviousMatches(null, 25).subscribe(res => {
      this.matches = new RollList<MatchStatus>(res.data, res.hasMoreLeft);

      this.subscription.add(this.observableHub.receiveResult.subscribe(status => {
        if (status.state === GameState.Won || status.state === GameState.Draw || status.state === GameState.Lost) {
          this.matches.elements.push(status);
          this.matchListUpdated.next();
        }
      }));
    });
  }

  onLoadMore() {
    const oldestId = this.matches.elements[this.matches.elements.length - 1].id;
    this.matchService.getPreviousMatches(oldestId, 25).subscribe(res => {
      this.matches.hasMore = res.hasMoreLeft;
      this.matches.elements = this.matches.elements.concat(res.data);
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
