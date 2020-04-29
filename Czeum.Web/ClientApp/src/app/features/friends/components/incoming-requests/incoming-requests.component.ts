import { Component, OnDestroy, OnInit } from '@angular/core';
import { FriendsService } from '../../../../shared/services/friends.service';
import { FriendDto, FriendRequestDto } from '../../../../shared/clients';
import { HubService } from '../../../../shared/services/hub.service';
import { MatDialog } from '@angular/material';
import { ConfirmDialogComponent } from '../../../../shared/components/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogResult } from '../../../../shared/models/dialog.models';

@Component({
  selector: 'app-incoming-requests',
  templateUrl: './incoming-requests.component.html',
  styleUrls: ['./incoming-requests.component.scss']
})
export class IncomingRequestsComponent implements OnInit, OnDestroy {
  requests: FriendRequestDto[];
  filteredRequests: FriendRequestDto[];
  filterText = '';

  constructor(
    private friendsService: FriendsService,
    private hubService: HubService,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    this.friendsService.getIncomingFriendRequests().subscribe(res => {
      this.requests = res;

      this.hubService.registerCallback('ReceiveRequest', (request: FriendRequestDto) => {
        this.requests.push(request);
        this.filterRequests();
      });

      this.hubService.registerCallback('RequestRevoked', (requestId: string) => {
        const index = this.requests.findIndex(x => x.id === requestId);
        if (index !== -1) {
          this.requests.splice(index, 1);
          this.filterRequests();
        }
      });

      this.filteredRequests = this.requests;
    });
  }

  ngOnDestroy() {
    this.hubService.removeCallback('ReceiveRequest');
    this.hubService.removeCallback('RequestRevoked');
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
    this.friendsService.acceptRequest(request.id).subscribe(() => {
      this.deleteRequestById(request.id);
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
