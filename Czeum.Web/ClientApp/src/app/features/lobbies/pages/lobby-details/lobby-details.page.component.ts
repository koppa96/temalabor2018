import { AfterViewInit, Component, OnDestroy, OnInit, QueryList, ViewChildren } from '@angular/core';
import { Observable, Subject, Subscription } from 'rxjs';
import { LobbyDataWrapper, Message } from '../../../../shared/clients';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { LobbyService } from '../../services/lobby.service';
import { take } from 'rxjs/operators';
import { leaveLobby, updateLobby } from '../../../../reducers/current-lobby/current-lobby-actions';
import { LobbyCreateDetails } from '../../models/lobby-create.models';
import { RollList } from '../../../../shared/models/roll-list';
import { LobbyChatComponent } from '../../components/lobby-chat/lobby-chat.component';
import { Router } from '@angular/router';
import { ObservableHub } from '../../../../shared/services/observable-hub.service';

@Component({
  templateUrl: './lobby-details.page.component.html',
  styleUrls: ['./lobby-details.page.component.scss']
})
export class LobbyDetailsPageComponent implements OnInit, OnDestroy {
  messages: RollList<Message>;
  currentLobby$: Observable<LobbyDataWrapper>;
  isLoading = false;
  isSending = false;

  subscription = new Subscription();
  messageReceived = new Subject();

  constructor(
    private store: Store<State>,
    private observableHub: ObservableHub,
    private lobbyService: LobbyService,
    private router: Router
  ) {
    this.currentLobby$ = store.select(x => x.currentLobby);
  }

  ngOnInit() {
    this.isLoading = true;
    this.lobbyService.getCurrentLobby().subscribe(res => {
      if (res === null) {
        this.store.dispatch(leaveLobby());
      } else {
        this.store.dispatch(updateLobby({ newLobby: res }));

        this.lobbyService.getLobbyMessages(res.content.id, 25).subscribe(messages => {
          this.messages = messages;
          this.subscription.add(this.observableHub.receiveLobbyMessage.subscribe(args => {
            this.messages.elements.push(args.message);
            this.messageReceived.next();
          }));
        });

        this.subscription.add(this.observableHub.matchCreated.subscribe(match => {
          this.store.dispatch(leaveLobby());
          this.router.navigate(['games', match.id, match.currentBoard.gameIdentifier]);
        }));
      }
      this.isLoading = false;
    },
      () => {
        this.store.dispatch(leaveLobby());
        this.isLoading = false;
      }
    );
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  onLobbyCreate(details: LobbyCreateDetails) {
    this.lobbyService.createLobby(details).subscribe(res => {
      this.store.dispatch(updateLobby({ newLobby: res }));
      this.subscription = this.observableHub.receiveLobbyMessage.subscribe(args => {
        this.messages.elements.push(args.message);
        this.messageReceived.next();
      });
    });
  }

  onLobbyLeave() {
    this.lobbyService.leaveLobby().subscribe(() => {
      this.store.dispatch(leaveLobby());
      this.messages = new RollList<Message>();
    });
  }

  onMessageSending(newMessage: string) {
    this.isSending = true;
    this.currentLobby$.pipe(
      take(1)
    ).subscribe(lobby => {
      this.lobbyService.sendMessage(lobby.content.id, newMessage).subscribe(res => {
        this.messages.elements.push(res);
        this.isSending = false;
      });
    });
  }

  startMatch() {
    this.currentLobby$.pipe(
      take(1)
    ).subscribe(lobby => {
      this.lobbyService.createMatchFromLobby(lobby.content.id).subscribe(match => {
        this.router.navigate(['games', match.id, match.currentBoard.gameIdentifier]).then(() => {
          this.store.dispatch(leaveLobby());
        });
      });
    });
  }

  updateLobby(lobby: LobbyDataWrapper) {
    this.lobbyService.updateLobby(lobby).subscribe(res => {
      this.store.dispatch(updateLobby({ newLobby: res }));
    });
  }

  loadMoreMessages() {
    this.currentLobby$.pipe(
      take(1)
    ).subscribe(lobby => {
      const oldestMessage = this.messages.elements[0];
      this.lobbyService.getLobbyMessages(lobby.content.id, 25, oldestMessage.id).subscribe(res => {
        this.messages.hasMore = res.hasMore;
        this.messages.elements = res.elements.concat(this.messages.elements);
      });
    });
  }
}
