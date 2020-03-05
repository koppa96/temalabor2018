import { Component, Input, OnInit } from '@angular/core';
import { FriendListItem } from '../../models/friend-list.models';

@Component({
  selector: 'app-friend-list-item',
  templateUrl: './friend-list-item.component.html',
  styleUrls: ['./friend-list-item.component.scss']
})
export class FriendListItemComponent implements OnInit {
  @Input() friendListItem: FriendListItem;

  constructor() { }

  ngOnInit() {
  }

}
