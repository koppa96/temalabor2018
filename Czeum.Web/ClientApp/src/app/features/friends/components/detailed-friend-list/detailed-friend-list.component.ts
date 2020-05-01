import { Component, OnDestroy, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from 'src/app/reducers';
import { Observable, Subscription } from 'rxjs';
import { FriendDto } from 'src/app/shared/clients';
import { detailedFriendListOrderings } from '../../models/ordering.models';
import { Ordering } from '../../../../shared/models/ordering.models';
import { getLastOnlineText } from '../../../../shared/services/date-utils';

@Component({
  selector: 'app-detailed-friend-list',
  templateUrl: './detailed-friend-list.component.html',
  styleUrls: ['./detailed-friend-list.component.scss']
})
export class DetailedFriendListComponent implements OnInit, OnDestroy {
  filteredFriendList: FriendDto[] = [];
  friendList: FriendDto[] = [];
  filterText = '';

  orderings = detailedFriendListOrderings;
  selectedOrdering: Ordering<FriendDto>;

  subscription: Subscription;

  constructor(
    private store: Store<State>
  ) {

  }

  ngOnInit() {
    this.selectedOrdering = this.orderings[0];
    this.subscription = this.store.select(x => x.friendList).subscribe(result => {
      this.friendList = result;
      this.filterAndSortFriends();
    });
  }

  filterAndSortFriends() {
    this.filteredFriendList = this.friendList.filter(x => x.username.toLowerCase().includes(this.filterText.toLowerCase()))
      .sort(this.selectedOrdering.comparator);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  getLastOnlineText(friend: FriendDto): string {
    return getLastOnlineText(friend.lastDisconnect);
  }

}
