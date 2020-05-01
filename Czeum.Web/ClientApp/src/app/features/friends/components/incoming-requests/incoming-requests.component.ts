import { Component, OnDestroy, OnInit } from '@angular/core';
import { FriendsService } from '../../../../shared/services/friends.service';
import { FriendRequestDto } from '../../../../shared/clients';
import { MatDialog } from '@angular/material';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogResult } from '../../../../shared/models/dialog.models';
import { ObservableHub } from '../../../../shared/services/observable-hub.service';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { addFriend } from '../../../../reducers/friend-list/friend-list-actions';

@Component({
  selector: 'app-incoming-requests',
  templateUrl: './incoming-requests.component.html',
  styleUrls: ['./incoming-requests.component.scss']
})
export class IncomingRequestsComponent implements OnInit, OnDestroy {
  requests: FriendRequestDto[];
  filteredRequests: FriendRequestDto[];
  filterText = '';

  subscription = new Subscription();

  constructor(
    private friendsService: FriendsService,
    private observableHub: ObservableHub,
    private dialog: MatDialog,
    private store: Store<State>
  ) { }

  ngOnInit() {
    this.friendsService.getIncomingFriendRequests().subscribe(res => {
      this.requests = res;

      this.subscription.add(this.observableHub.receiveRequest.subscribe(request => {
        this.requests.push(request);
        this.filterRequests();
      }));

      this.subscription.add(this.observableHub.requestRevoked.subscribe(requestId => {
        const index = this.requests.findIndex(x => x.id === requestId);
        if (index !== -1) {
          this.requests.splice(index, 1);
          this.filterRequests();
        }
      }));

      this.filteredRequests = this.requests;
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  filterRequests() {
    this.filteredRequests = this.requests.filter(x => x.receiverName.toLowerCase().includes(this.filterText.toLowerCase()));
  }

  onReject(request: FriendRequestDto) {
    this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Megerősítés szükséges',
        text: `Biztosan törölni szeretnéd a(z) ${request.senderName} által küldött kérelmet?`,
        autoFocus: false
      }
    }).afterClosed().subscribe((result: ConfirmDialogResult) => {
      if (result && result.shouldProceed) {
        this.friendsService.rejectRequest(request.id).subscribe(() => {
          this.deleteRequestById(request.id);
        });
      }
    });
  }

  onAccept(request: FriendRequestDto) {
    this.friendsService.acceptRequest(request.id).subscribe(friend => {
      this.deleteRequestById(request.id);
      this.store.dispatch(addFriend({ friend }));
    });
  }

  deleteRequestById(id: string) {
    const index = this.requests.findIndex(x => x.id === id);
    if (index !== -1) {
      this.requests.splice(index, 1);
      this.filterRequests();
    }
  }

}
