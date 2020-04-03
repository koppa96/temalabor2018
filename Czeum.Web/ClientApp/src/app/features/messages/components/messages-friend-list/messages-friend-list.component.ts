import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { Observable } from 'rxjs';
import { FriendListItem } from '../../../../shared/models/friend-list.models';

@Component({
  selector: 'app-messages-friend-list',
  templateUrl: './messages-friend-list.component.html',
  styleUrls: ['./messages-friend-list.component.scss']
})
export class MessagesFriendListComponent implements OnInit {
  @Output() friendSelected = new EventEmitter<FriendListItem>();

  friendList$: Observable<FriendListItem[]>;

  constructor(private store: Store<State>) {
    this.friendList$ = store.select(x => x.friendList);
  }

  ngOnInit() {
  }

  getFriendStateString(friend: FriendListItem): string {
    const today = new Date();
    if (friend.isOnline) {
      return 'Online';
    } else if (today.getTime() - friend.lastDisconnect.getTime() < 86400000) {
      const elapsedMinutes = Math.floor((today.getTime() - friend.lastDisconnect.getTime()) / 60000);

      if (elapsedMinutes < 10) {
        return 'Legutóbb online: néhány perce';
      } else if (elapsedMinutes < 60) {
        return `Legutóbb online: ${elapsedMinutes} perce`;
      } else if (elapsedMinutes / 60 < 24) {
        return `Legutóbb online: ${Math.floor(elapsedMinutes / 60)} órája`;
      } else {
        return `Legutóbb online: ${
          friend.lastDisconnect.getFullYear()}. ${friend.lastDisconnect.getMonth() + 1 < 10 ? `0${friend.lastDisconnect.getMonth() + 1}` : friend.lastDisconnect.getMonth() + 1}. ${friend.lastDisconnect.getDate()}`;
      }
    }
  }

  selectFriend(friend: FriendListItem) {
    this.friendSelected.emit(friend);
  }

}
