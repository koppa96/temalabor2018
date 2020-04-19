import { Component, OnDestroy, OnInit } from '@angular/core';
import { HubService } from '../../../../shared/services/hub.service';
import { MatchService } from '../../services/match.service';
import { GameState, MatchStatus } from '../../../../shared/clients';
import { RollList } from '../../../../shared/models/roll-list';

@Component({
  selector: 'app-current-game-list.page',
  templateUrl: './current-game-list.page.component.html',
  styleUrls: ['./current-game-list.page.component.scss']
})
export class CurrentGameListPageComponent implements OnInit, OnDestroy {
  matches: RollList<MatchStatus>;

  constructor(private hubService: HubService, private matchService: MatchService) { }

  private onMatchCreated(matchStatus: MatchStatus) {
    this.matches.elements.splice(0, 0, matchStatus);
  }

  private onMatchUpdated(matchStatus: MatchStatus) {
    const currentIndex = this.matches.elements.findIndex(x => x.id === matchStatus.id);
    if (matchStatus.state === GameState.Draw || matchStatus.state === GameState.Won || matchStatus.state === GameState.Lost) {
      this.matches.elements.splice(currentIndex, 1);
    } else {
      this.matches.elements[currentIndex] = matchStatus;
    }
  }

  ngOnInit() {
    this.matchService.getCurrentMatches(null, 25).subscribe(res => {
      this.matches = new RollList<MatchStatus>(res.data, res.hasMoreLeft);

      this.hubService.registerCallback('MatchCreated', this.onMatchCreated);
      this.hubService.registerCallback('ReceiveResult', this.onMatchUpdated);
    });
  }

  ngOnDestroy() {
    this.hubService.removeCallback('MatchCreated');
    this.hubService.removeCallback('ReceiveResult');
  }

}
