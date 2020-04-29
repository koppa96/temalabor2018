import { Component, OnInit, OnDestroy } from '@angular/core';
import { FriendsService } from 'src/app/shared/services/friends.service';
import { FriendRequestDto, UserDto } from 'src/app/shared/clients';
import { HubService } from 'src/app/shared/services/hub.service';
import { MatDialog } from '@angular/material';
import { ConfirmDialogComponent } from 'src/app/shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogResult } from 'src/app/shared/models/dialog.models';
import { AuthService } from '../../../../authentication/services/auth.service';
import { take } from 'rxjs/operators';
import { NewFriendRequestDialogComponent } from '../new-friend-request-dialog/new-friend-request-dialog.component';
import { NewRequestDialogResult } from '../../models/new-request-dialog.models';

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

  constructor(
    private friendsService: FriendsService,
    private hubService: HubService,
    private dialog: MatDialog,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.isLoading = true;
    this.friendsService.getOutgoingFriendRequests().subscribe(res => {
      this.requests = res;
      this.hubService.registerCallback('RequestRejected', (requestId: string) => {
        const index = this.requests.findIndex(x => x.id === requestId);
        if (index !== -1) {
          this.requests.splice(index, 1);
        }
        this.filterRequests();
      });
      this.isLoading = false;
      this.filterRequests();
    });

    this.friendsService.getUsernameAutocomplete().subscribe(res => {
      this.autocompleteContent = res;
    });
  }

  ngOnDestroy() {
    this.hubService.removeCallback('RequestRejected');
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

  onNewRequestClicked() {
    this.authService.getAuthState().pipe(
      take(1)
    ).subscribe(authState => {
      console.log(this.requests);
      console.log(this.autocompleteContent);
      const selectableUsers = this.autocompleteContent.filter(x => x.id !== authState.profile.userId &&
        !this.requests.some(r => r.receiverName === x.username));

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
  }

}
