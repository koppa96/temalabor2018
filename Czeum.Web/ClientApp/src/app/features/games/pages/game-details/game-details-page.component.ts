import { Component, OnInit } from '@angular/core';
import { ObservableHub } from '../../../../shared/services/observable-hub.service';
import { ActivatedRoute } from '@angular/router';
import { MatchService } from '../../services/match.service';
import { MatchStatus, Message } from '../../../../shared/clients';
import { RollList } from '../../../../shared/models/roll-list';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details-page.component.html',
  styleUrls: ['./game-details-page.component.scss']
})
export class GameDetailsPageComponent implements OnInit {
  messagesLoading = false;
  messages: RollList<Message> = new RollList<Message>();

  constructor(
    private observableHub: ObservableHub,
    private route: ActivatedRoute,
    private matchService: MatchService
  ) { }

  ngOnInit() {
    const matchId = this.route.snapshot.params.matchId;

    this.messagesLoading = true;
    this.matchService.getMatchMessages(matchId).pipe(
      finalize(() => this.messagesLoading = false)
    ).subscribe(messages => {
      this.messages = messages;
    });
  }
}
