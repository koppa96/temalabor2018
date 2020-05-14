import { AfterViewInit, Component, EventEmitter, Input, OnInit, Output, QueryList, ViewChildren } from '@angular/core';
import { RollList } from '../../../../shared/models/roll-list';
import { Message } from '../../../../shared/clients';
import { Profile } from '../../../../authentication/auth-config-models';
import { PerfectScrollbarComponent } from 'ngx-perfect-scrollbar';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-game-chat',
  templateUrl: './game-chat.component.html',
  styleUrls: ['./game-chat.component.scss']
})
export class GameChatComponent implements OnInit, AfterViewInit {
  @Input() isSending = false;
  @Input() currentUser: Profile;
  @Input() messages: RollList<Message> = new RollList<Message>();

  @Output() loadMore = new EventEmitter();
  @Output() newMessage = new EventEmitter<string>();

  @ViewChildren('scrollbar') scrollbars: QueryList<PerfectScrollbarComponent>;
  scrollbar: PerfectScrollbarComponent;

  newMessageText = '';

  constructor() {
  }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.scrollbars.changes.subscribe((components: QueryList<PerfectScrollbarComponent>) => {
      this.scrollbar = components.first;
      this.scrollbar.directiveRef.scrollToBottom(0);
    });
  }

  onTyping(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.onSend();
    }
  }

  onSend(event?: MouseEvent) {
    if (event) {
      event.stopPropagation();
    }
    this.newMessage.emit(this.newMessageText);
    this.newMessageText = '';
  }
}
