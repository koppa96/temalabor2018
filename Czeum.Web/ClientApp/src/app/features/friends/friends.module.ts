import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FriendsPageComponent } from './pages/friends.page.component';
import { FriendsRoutingModule } from './friends-routing.module';
import { DetailedFriendListComponent } from './components/detailed-friend-list/detailed-friend-list.component';
import { IncomingRequestsComponent } from './components/incoming-requests/incoming-requests.component';
import { OutgoingRequestsComponent } from './components/outgoing-requests/outgoing-requests.component';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [FriendsPageComponent, DetailedFriendListComponent, IncomingRequestsComponent, OutgoingRequestsComponent],
  imports: [
    CommonModule,
    FriendsRoutingModule,
    SharedModule,
    FormsModule
  ]
})
export class FriendsModule { }
