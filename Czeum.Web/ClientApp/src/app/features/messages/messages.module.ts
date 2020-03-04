import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessagesPageComponent } from './pages/messages.page/messages.page.component';
import { MessagesRoutingModule } from './messages-routing.module';

@NgModule({
  declarations: [
    MessagesPageComponent
  ],
  imports: [
    CommonModule,
    MessagesRoutingModule
  ]
})
export class MessagesModule { }
