import { Component, OnDestroy, OnInit } from '@angular/core';
import { ObservableHub } from '../../../../shared/services/observable-hub.service';
import { ActivatedRoute } from '@angular/router';
import { MatchService } from '../../services/match.service';
import { MatchStatus, Message } from '../../../../shared/clients';
import { RollList } from '../../../../shared/models/roll-list';
import { finalize, take } from 'rxjs/operators';
import { AuthService } from '../../../../authentication/services/auth.service';
import { Observable, Subject, Subscription } from 'rxjs';
import { AuthState } from '../../../../reducers';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details-page.component.html',
  styleUrls: ['./game-details-page.component.scss']
})
export class GameDetailsPageComponent implements OnInit, OnDestroy {
  messagesLoading = false;
  messages: RollList<Message> = new RollList<Message>();
  authState$: Observable<AuthState>;
  isSending = false;
  matchId: string;

  subscription = new Subscription();

  constructor(
    private observableHub: ObservableHub,
    private route: ActivatedRoute,
    private matchService: MatchService,
    private authService: AuthService
  ) {
    this.authState$ = this.authService.getAuthState();
  }

  ngOnInit() {
    this.matchId = this.route.snapshot.params.matchId;

    this.messagesLoading = true;
    this.matchService.getMatchMessages(this.matchId).pipe(
      finalize(() => this.messagesLoading = false)
    ).subscribe(messages => {
      this.messages = messages;
      this.subscription = this.observableHub.receiveMatchMessage.subscribe(args => {
        if (args.id === this.matchId) {
          this.messages.elements.push(args.message);
        }
      });
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  onMessageSending(newMessage: string) {
    this.matchService.sendMatchMessage(this.matchId, newMessage).subscribe(res => {
      this.messages.elements.push(res);
      this.isSending = false;
    });
  }

  loadMoreMessages() {
    const oldestMessage = this.messages.elements[0];
    this.matchService.getMatchMessages(this.matchId, 25, oldestMessage.id).subscribe(res => {
      this.messages.hasMore = res.hasMore;
      this.messages.elements = res.elements.concat(this.messages.elements);
    });
  }
}
