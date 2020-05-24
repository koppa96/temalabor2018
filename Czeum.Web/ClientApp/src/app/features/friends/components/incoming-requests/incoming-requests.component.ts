import { Component, OnDestroy, OnInit } from '@angular/core';
import { FriendsService } from '../../../../shared/services/friends.service';
import { FriendRequestDto, NotificationType } from '../../../../shared/clients';
import { MatDialog } from '@angular/material';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogResult } from '../../../../shared/models/dialog.models';
import { ObservableHub } from '../../../../shared/services/observable-hub.service';
import { BehaviorSubject, combineLatest, Observable, Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { addFriend } from '../../../../reducers/friend-list/friend-list-actions';
import {
  addIncomingFriendRequest,
  removeIncomingFriendRequest,
  updateIncomingFriendRequestList
} from '../../../../reducers/friend-list/incoming-friend-requests-actions';
import { FormControl } from '@angular/forms';
import { map, take } from 'rxjs/operators';
import { NotificationService } from '../../../../shared/services/notification.service';
import { deleteNotification } from '../../../../reducers/notifications/notifications-actions';

@Component({
  selector: 'app-incoming-requests',
  templateUrl: './incoming-requests.component.html',
  styleUrls: ['./incoming-requests.component.scss']
})
export class IncomingRequestsComponent implements OnInit, OnDestroy {
  requests$: Observable<FriendRequestDto[]>;
  filteredRequests$: Observable<FriendRequestDto[]>;
  filterText = new FormControl('');
  filterText$ = new BehaviorSubject('');

  subscription = new Subscription();

  constructor(
    private friendsService: FriendsService,
    private observableHub: ObservableHub,
    private dialog: MatDialog,
    private store: Store<State>,
    private notificationService: NotificationService
  ) {
    this.requests$ = this.store.select(x => x.incomingFriendRequests);

    this.filterText.valueChanges.subscribe(value => this.filterText$.next(value));
    this.filteredRequests$ = combineLatest([this.requests$, this.filterText$]).pipe(
      map(([requests, filterText]) => {
        return requests.filter(x => x.senderName.toLowerCase().includes(filterText.toLowerCase()));
      })
    );
  }

  ngOnInit() {
    this.friendsService.getIncomingFriendRequests().subscribe(res => {
      this.store.dispatch(updateIncomingFriendRequestList({ requests: res }));
      this.subscription.add(this.observableHub.receiveRequest.subscribe(request => {
        this.store.dispatch(addIncomingFriendRequest({ request }));
      }));

      this.subscription.add(this.observableHub.requestRevoked.subscribe(requestId => {
        this.store.dispatch(removeIncomingFriendRequest({ requestId }));
      }));
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
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
          this.store.dispatch(removeIncomingFriendRequest({ requestId: request.id }));
          this.deleteNotificationForRequest(request);
        });
      }
    });
  }

  onAccept(request: FriendRequestDto) {
    this.friendsService.acceptRequest(request.id).subscribe(friend => {
      this.store.dispatch(removeIncomingFriendRequest({ requestId: request.id }));
      this.store.dispatch(addFriend({ friend }));
      this.deleteNotificationForRequest(request);
    });
  }

  private deleteNotificationForRequest(request: FriendRequestDto) {
    this.store.select(x => x.notifications).pipe(
      take(1)
    ).subscribe(notifications => {
      const notification = notifications.find(x => x.type === NotificationType.FriendRequestReceived && x.data === request.id);
      if (notification) {
        this.notificationService.deleteNotification(notification.id).subscribe(() => {
          this.store.dispatch(deleteNotification({ id: notification.id }));
        });
      }
    });
  }
}
