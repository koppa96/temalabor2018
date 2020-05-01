import { Component, OnInit, OnDestroy } from '@angular/core';
import { FriendsService } from 'src/app/shared/services/friends.service';
import { FriendRequestDto, UserDto } from 'src/app/shared/clients';
import { MatDialog } from '@angular/material';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogResult } from 'src/app/shared/models/dialog.models';
import { AuthService } from '../../../../authentication/services/auth.service';
import { take } from 'rxjs/operators';
import { NewFriendRequestDialogComponent } from '../new-friend-request-dialog/new-friend-request-dialog.component';
import { NewRequestDialogResult } from '../../models/new-request-dialog.models';
import { ObservableHub } from '../../../../shared/services/observable-hub.service';
import { Subscription } from 'rxjs';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';

@Component({
  selector: 'app-outgoing-requests',
  templateUrl: './outgoing-requests.component.html',
  styleUrls: ['./outgoing-requests.component.scss']
})
export class OutgoingRequestsComponent implements OnInit, OnDestroy {
  isLoading = false;
  filteredRequests: FriendRequestDto[] = [];
  requests: FriendRequestDto[] = [];
  autocompleteContent: UserDto[] = [];
  filterText = '';

  subscription = new Subscription();

  constructor(
    private friendsService: FriendsService,
    private observableHub: ObservableHub,
    private dialog: MatDialog,
    private authService: AuthService,
    private store: Store<State>
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.friendsService.getOutgoingFriendRequests().subscribe(res => {
      this.requests = res;
      this.subscription.add(this.observableHub.requestRejected.subscribe(requestId => {
        const index = this.requests.findIndex(x => x.id === requestId);
        if (index !== -1) {
          this.requests.splice(index, 1);
        }
        this.filterRequests();
      }));

      this.subscription.add(this.observableHub.friendAdded.subscribe(friend => {
        const index = this.requests.findIndex(x => x.senderName === friend.username || x.receiverName === friend.username);
        if (index !== -1) {
          this.requests.splice(index, 1);
        }
        this.filterRequests();
      }));
      this.isLoading = false;
      this.filterRequests();
    });

    this.friendsService.getUsernameAutocomplete().subscribe(res => {
      this.autocompleteContent = res;
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  filterRequests() {
    this.filteredRequests = this.requests.filter(x => x.receiverName.toLowerCase().includes(this.filterText.toLowerCase()));
  }

  onDeleteClicked(request: FriendRequestDto) {
    this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Megerősítés szükséges',
        text: `Biztosan törölni szeretnéd a(z) ${request.receiverName} felhasználónak küldött kérelmed?`,
        autoFocus: false
      }
    }).afterClosed().subscribe((result: ConfirmDialogResult | null) => {
      if (result && result.shouldProceed) {
        this.friendsService.cancelFriendRequest(request.id).subscribe(() => {
          const index = this.requests.findIndex(x => x.id === request.id);
          this.requests.splice(index, 1);
          this.filterRequests();
        });
      }
    });
  }

  async onNewRequestClicked() {
    this.authService.getAuthState().pipe(
      take(1)
    ).subscribe(authState => {
      this.store.select(x => x.friendList).pipe(
        take(1)
      ).subscribe(friends => {
        const selectableUsers = this.autocompleteContent.filter(x => x.id !== authState.profile.userId &&
          !this.requests.some(r => r.receiverName === x.username) && !friends.some(f => f.username === x.username));

        this.dialog.open(NewFriendRequestDialogComponent, {
          data: {
            users: selectableUsers
          },
          autoFocus: false
        }).afterClosed().subscribe((result: NewRequestDialogResult) => {
          if (result) {
            this.friendsService.sendFriendRequest(result.selectedUser.id).subscribe(request => {
              this.requests.push(request);
              this.filterRequests();
            });
          }
        });
      });
    });
  }

}
