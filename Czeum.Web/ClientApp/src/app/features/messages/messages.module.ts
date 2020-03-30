import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessagesPageComponent } from './pages/messages/messages.page.component';
import { MessagesRoutingModule } from './messages-routing.module';
import { MessagesFriendListComponent } from './components/messages-friend-list/messages-friend-list.component';
import { ChatComponent } from './components/chat/chat.component';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    MessagesPageComponent,
    MessagesFriendListComponent,
    ChatComponent
  ],
  imports: [
    CommonModule,
    MessagesRoutingModule,
    SharedModule
  ]
})
export class MessagesModule { }
