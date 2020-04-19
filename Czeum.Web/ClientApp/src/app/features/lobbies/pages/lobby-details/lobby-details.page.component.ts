import { AfterViewInit, Component, OnDestroy, OnInit, QueryList, ViewChildren } from '@angular/core';
import { Observable } from 'rxjs';
import { LobbyDataWrapper, Message } from '../../../../shared/clients';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { HubService } from '../../../../shared/services/hub.service';
import { LobbyService } from '../../services/lobby.service';
import { take } from 'rxjs/operators';
import { leaveLobby, updateLobby } from '../../../../reducers/current-lobby/current-lobby-actions';
import { LobbyCreateDetails } from '../../models/lobby-create.models';
import { RollList } from '../../../../shared/models/roll-list';
import { LobbyChatComponent } from '../../components/lobby-chat/lobby-chat.component';
import { Router } from '@angular/router';

@Component({
  templateUrl: './lobby-details.page.component.html',
  styleUrls: ['./lobby-details.page.component.scss']
})
export class LobbyDetailsPageComponent implements OnInit, OnDestroy, AfterViewInit {
  messages: RollList<Message>;
  currentLobby$: Observable<LobbyDataWrapper>;
  isLoading = false;
  isSending = false;

  chatComponent: LobbyChatComponent;

  @ViewChildren('chat')
  chatComponents: QueryList<LobbyChatComponent>;

  constructor(
    private store: Store<State>,
    private hubService: HubService,
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
        this.store.dispatch(updateLobby({newLobby: res}));
        this.hubService.registerCallback('LobbyChanged', this.createLobbyChangedCallback());
        this.hubService.registerCallback('KickedFromLobby', this.createKickCallback());
        this.hubService.registerCallback('ReceiveLobbyMessage', this.createReceiveLobbyMessageCallback());

        this.lobbyService.getLobbyMessages(res.content.id, 25).subscribe(messages => {
          this.messages = messages;
        });
      }
      this.isLoading = false;
    },
      err => console.log(err));
  }

  ngAfterViewInit() {
    this.chatComponents.changes.subscribe(res => {
      this.chatComponent = res;
    });
  }

  ngOnDestroy() {
    this.hubService.removeCallback('LobbyChanged');
    this.hubService.removeCallback('KickedFromLobby');
    this.hubService.removeCallback('ReceiveLobbyMessage');
  }

  onLobbyCreate(details: LobbyCreateDetails) {
    this.lobbyService.createLobby(details).subscribe(res => {
      this.store.dispatch(updateLobby({ newLobby: res }));
      this.hubService.registerCallback('LobbyChanged', this.createLobbyChangedCallback());
      this.hubService.registerCallback('KickedFromLobby', this.createKickCallback());
      this.hubService.registerCallback('ReceiveLobbyMessage', this.createReceiveLobbyMessageCallback());
    });
  }

  onLobbyLeave() {
    this.lobbyService.leaveLobby().subscribe(() => {
      this.store.dispatch(leaveLobby());
    });
  }

  onMessageSending(newMessage: string) {
    this.isSending = true;
    this.currentLobby$.pipe(
      take(1)
    ).subscribe(lobby => {
      this.isSending = false;
      this.lobbyService.sendMessage(lobby.content.id, newMessage).subscribe(res => {
        this.messages.elements.push(res);
      });
    });
  }

  startMatch() {
    this.currentLobby$.pipe(
      take(1)
    ).subscribe(lobby => {
      this.lobbyService.createMatchFromLobby(lobby.content.id).subscribe(match => {
        this.router.navigate(['/']).then(() => {
          this.store.dispatch(leaveLobby());
        });
      });
    });
  }

  createLobbyChangedCallback(): (lobby: LobbyDataWrapper) => void {
    const self = this;
    return lobby => {
      self.currentLobby$.pipe(
        take(1)
      ).subscribe(currentLobby => {
        if (lobby.content.id === currentLobby.content.id) {
          self.store.dispatch(updateLobby({ newLobby: lobby }));
        }
      });
    };
  }

  createKickCallback(): () => void {
    const self = this;
    return () => {
      self.store.dispatch(leaveLobby());
      self.hubService.removeCallback('LobbyChanged');
      self.hubService.removeCallback('KickedFromLobby');
      self.hubService.removeCallback('ReceiveLobbyMessage');
    };
  }

  createReceiveLobbyMessageCallback(): (lobbyId: string, message: Message) => void {
    const self = this;
    return (lobbyId, message) => {
      self.currentLobby$.pipe(
        take(1)
      ).subscribe(currentLobby => {
        if (currentLobby.content.id === lobbyId) {
          self.messages.elements.push(message);
          self.chatComponent.scrollToBottom();
        }
      });
    };
  }
}
