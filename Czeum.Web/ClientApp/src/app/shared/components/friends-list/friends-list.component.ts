import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from '../../../reducers';
import { Observable } from 'rxjs';
import { FriendDto } from '../../clients';

@Component({
  selector: 'app-friends-list',
  templateUrl: './friends-list.component.html',
  styleUrls: ['./friends-list.component.scss']
})
export class FriendsListComponent implements OnInit {
  friendList$: Observable<FriendDto[]>;

  constructor(private store: Store<State>) { }

  ngOnInit() {
    this.friendList$ = this.store.select(s => s.friendList);
  }

}
