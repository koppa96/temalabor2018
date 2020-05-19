import {
  AfterViewInit,
  Component,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  QueryList,
  ViewChild,
  ViewChildren
} from '@angular/core';
import { Message } from '../../../../shared/clients';
import { RollList } from '../../../../shared/models/roll-list';
import { PerfectScrollbarComponent } from 'ngx-perfect-scrollbar';
import { AuthService } from '../../../../authentication/services/auth.service';
import { Observable, Subscription } from 'rxjs';
import { AuthState } from '../../../../reducers';

@Component({
  selector: 'app-lobby-chat',
  templateUrl: './lobby-chat.component.html',
  styleUrls: ['./lobby-chat.component.scss']
})
export class LobbyChatComponent implements OnInit, AfterViewInit, OnDestroy {
  @Input() isSending = false;
  @Input() messages = new RollList<Message>();
  @Input() messageReceived: Observable<void>;
  @Output() newMessage = new EventEmitter<string>();
  @Output() loadMore = new EventEmitter();

  scrollbar: PerfectScrollbarComponent;

  @ViewChildren('scrollbar')
  scrollbars: QueryList<PerfectScrollbarComponent>;

  newMessageText: string;
  authState$: Observable<AuthState>;
  subscription = new Subscription();

  constructor(private authService: AuthService) {
    this.authState$ = this.authService.getAuthState();
  }

  ngOnInit() {
    this.subscription.add(this.messageReceived.subscribe(() => this.scrollToBottom()));
  }

  ngAfterViewInit() {
    this.subscription.add(this.scrollbars.changes.subscribe((components: QueryList<PerfectScrollbarComponent>) => {
      if (components.first) {
        this.scrollbar = components.first;
        this.scrollbar.directiveRef.scrollToBottom(0);
      }
    }));
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  onSend(event?: MouseEvent) {
    if (event) {
      event.stopPropagation();
    }

    this.newMessage.emit(this.newMessageText);
    this.newMessageText = '';
  }

  onTyping(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.onSend();
    }
  }

  scrollToBottom() {
    if (this.scrollbar) {
      this.scrollbar.directiveRef.scrollToBottom(0);
    }
  }
}
