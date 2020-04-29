import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FriendsPageComponent } from './pages/friends.page.component';
import { FriendsRoutingModule } from './friends-routing.module';
import { DetailedFriendListComponent } from './components/detailed-friend-list/detailed-friend-list.component';
import { IncomingRequestsComponent } from './components/incoming-requests/incoming-requests.component';
import { OutgoingRequestsComponent } from './components/outgoing-requests/outgoing-requests.component';
import { SharedModule } from '../../shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NewFriendRequestDialogComponent } from './components/new-friend-request-dialog/new-friend-request-dialog.component';
import { MatAutocompleteModule } from '@angular/material';

@NgModule({
  declarations: [
    FriendsPageComponent,
    DetailedFriendListComponent,
    IncomingRequestsComponent,
    OutgoingRequestsComponent,
    NewFriendRequestDialogComponent
  ],
  imports: [
    CommonModule,
    FriendsRoutingModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
    MatAutocompleteModule
  ],
  entryComponents: [
    NewFriendRequestDialogComponent
  ]
})
export class FriendsModule { }
