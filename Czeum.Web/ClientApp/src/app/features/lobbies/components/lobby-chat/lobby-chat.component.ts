import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { Message } from '../../../../shared/clients';
import { RollList } from '../../../../shared/models/roll-list';
import { PerfectScrollbarComponent } from 'ngx-perfect-scrollbar';
import { AuthService } from '../../../../authentication/services/auth.service';
import { Observable } from 'rxjs';
import { AuthState } from '../../../../reducers';

@Component({
  selector: 'app-lobby-chat',
  templateUrl: './lobby-chat.component.html',
  styleUrls: ['./lobby-chat.component.scss']
})
export class LobbyChatComponent implements OnInit, AfterViewInit {
  @Input() isSending = false;
  @Input() messages: RollList<Message> = new RollList<Message>();
  @Output() newMessage = new EventEmitter<string>();
  @Output() loadMore = new EventEmitter();

  scrollbar: PerfectScrollbarComponent;

  @ViewChildren('scrollbar')
  scrollbars: QueryList<PerfectScrollbarComponent>;

  newMessageText: string;
  authState$: Observable<AuthState>;

  constructor(private authService: AuthService) {
    this.authState$ = this.authService.getAuthState();
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.scrollbars.changes.subscribe((components: QueryList<PerfectScrollbarComponent>) => {
      this.scrollbar = components.first;
      this.scrollbar.directiveRef.scrollToBottom(0);
    });
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
