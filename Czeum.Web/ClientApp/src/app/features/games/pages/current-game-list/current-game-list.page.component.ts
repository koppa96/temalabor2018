import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatchService } from '../../services/match.service';
import { GameState, GameTypeDto, MatchStatus } from '../../../../shared/clients';
import { RollList } from '../../../../shared/models/roll-list';
import { ObservableHub } from '../../../../shared/services/observable-hub.service';
import { Subject, Subscription } from 'rxjs';
import { toLocalDate } from '../../../../shared/services/date-utils';

@Component({
  selector: 'app-current-game-list.page',
  templateUrl: './current-game-list.page.component.html',
  styleUrls: ['./current-game-list.page.component.scss']
})
export class CurrentGameListPageComponent implements OnInit, OnDestroy {
  matches: RollList<MatchStatus> = new RollList<MatchStatus>();
  subscription = new Subscription();
  gameTypes: GameTypeDto[] = [];
  matchListUpdated = new Subject();

  constructor(
    private observableHub: ObservableHub,
    private matchService: MatchService
  ) { }

  ngOnInit() {
    this.matchService.getAvailableGameTypes().subscribe(res => {
      this.gameTypes = res;
    });

    this.matchService.getCurrentMatches(null, 25).subscribe(res => {
      this.matches = new RollList<MatchStatus>(res.data, res.hasMoreLeft);

      this.subscription.add(this.observableHub.matchCreated.subscribe(status => {
        this.matches.elements.splice(0, 0, status);
        this.matchListUpdated.next();
      }));

      this.subscription.add(this.observableHub.receiveResult.subscribe(status => {
        status.lastMoveDate = toLocalDate(status.lastMoveDate);
        status.createDate = toLocalDate(status.createDate);
        const currentIndex = this.matches.elements.findIndex(x => x.id === status.id);
        if (status.state === GameState.Draw || status.state === GameState.Won || status.state === GameState.Lost) {
          this.matches.elements.splice(currentIndex, 1);
        } else {
          this.matches.elements[currentIndex] = status;
        }
        this.matchListUpdated.next();
      }));
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
