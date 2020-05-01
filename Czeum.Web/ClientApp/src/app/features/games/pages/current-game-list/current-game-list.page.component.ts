import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatchService } from '../../services/match.service';
import { GameState, MatchStatus } from '../../../../shared/clients';
import { RollList } from '../../../../shared/models/roll-list';
import { ObservableHub } from '../../../../shared/services/observable-hub.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-current-game-list.page',
  templateUrl: './current-game-list.page.component.html',
  styleUrls: ['./current-game-list.page.component.scss']
})
export class CurrentGameListPageComponent implements OnInit, OnDestroy {
  matches: RollList<MatchStatus>;
  subscription = new Subscription();

  constructor(private observableHub: ObservableHub, private matchService: MatchService) { }

  ngOnInit() {
    this.matchService.getCurrentMatches(null, 25).subscribe(res => {
      this.matches = new RollList<MatchStatus>(res.data, res.hasMoreLeft);

      this.subscription.add(this.observableHub.matchCreated.subscribe(status => {
        this.matches.elements.splice(0, 0, status);
      }));

      this.subscription.add(this.observableHub.receiveResult.subscribe(status => {
        const currentIndex = this.matches.elements.findIndex(x => x.id === status.id);
        if (status.state === GameState.Draw || status.state === GameState.Won || status.state === GameState.Lost) {
          this.matches.elements.splice(currentIndex, 1);
        } else {
          this.matches.elements[currentIndex] = status;
        }
      }));
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }
}
