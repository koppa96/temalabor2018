import { Component, Input, OnInit } from '@angular/core';
import { Message } from '../../clients';
import { Store } from '@ngrx/store';
import { State } from '../../../reducers';

@Component({
  selector: 'app-chat-message',
  templateUrl: './chat-message.component.html',
  styleUrls: ['./chat-message.component.scss']
})
export class ChatMessageComponent implements OnInit {
  @Input() currentUser: string;
  @Input() message: Message;

  constructor() { }

  ngOnInit() {
  }

}
