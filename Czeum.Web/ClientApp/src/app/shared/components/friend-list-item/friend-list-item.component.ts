import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FriendDto, LobbyDataWrapper } from '../../clients';

@Component({
  selector: 'app-friend-list-item',
  templateUrl: './friend-list-item.component.html',
  styleUrls: ['./friend-list-item.component.scss']
})
export class FriendListItemComponent implements OnInit {
  @Input() friendListItem: FriendDto;
  @Input() currentLobby: LobbyDataWrapper;

  @Output() invite = new EventEmitter<FriendDto>();
  @Output() cancelInvite = new EventEmitter<FriendDto>();

  constructor() { }

  ngOnInit() {
  }

  isInvited() {
    return this.currentLobby && this.currentLobby.content.invitedPlayers.some(x => x === this.friendListItem.username);
  }

  canInvite() {
    return this.currentLobby && this.currentLobby.content.host !== this.friendListItem.username &&
      !this.currentLobby.content.guests.some(x => x === this.friendListItem.username) &&
      !this.currentLobby.content.invitedPlayers.some(x => x === this.friendListItem.username);
  }

  onInvite() {
    this.invite.emit(this.friendListItem);
  }

  onCancelInvite() {
    this.cancelInvite.emit(this.friendListItem);
  }
}
