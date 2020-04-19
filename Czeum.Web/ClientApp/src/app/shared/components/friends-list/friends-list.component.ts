import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from '../../../reducers';
import { Observable } from 'rxjs';
import { FriendListItem } from '../../models/friend-list.models';

@Component({
  selector: 'app-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.scss']
})
export class FriendsListComponent implements OnInit {
  friendList$: Observable<FriendListItem[]>;

  constructor(private store: Store<State>) { }

  ngOnInit() {
    this.friendList$ = this.store.select(s => s.friendList);
  }

}
