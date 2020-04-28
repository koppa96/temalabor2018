import { Component, Input, OnInit } from '@angular/core';
import { FriendDto } from '../../clients';

@Component({
  selector: 'app-friend-list-item',
  templateUrl: './friend-list-item.component.html',
  styleUrls: ['./friend-list-item.component.scss']
})
export class FriendListItemComponent implements OnInit {
  @Input() friendListItem: FriendDto;

  constructor() { }

  ngOnInit() {
  }

}
