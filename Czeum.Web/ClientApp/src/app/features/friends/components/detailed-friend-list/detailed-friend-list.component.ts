import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from 'src/app/reducers';
import { Observable } from 'rxjs';
import { FriendDto } from 'src/app/shared/clients';

@Component({
  selector: 'app-detailed-friend-list',
  templateUrl: './detailed-friend-list.component.html',
  styleUrls: ['./detailed-friend-list.component.scss']
})
export class DetailedFriendListComponent implements OnInit {
  friendList$: Observable<FriendDto[]>;
  filterText: string;

  constructor(private store: Store<State>) { }

  ngOnInit() {

  }

}
