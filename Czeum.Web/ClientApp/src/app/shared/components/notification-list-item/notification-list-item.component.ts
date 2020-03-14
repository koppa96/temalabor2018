import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Notification } from '../../models/notification.models';

@Component({
  selector: 'app-notification-list-item',
  templateUrl: './notification-list-item.component.html',
  styleUrls: ['./notification-list-item.component.scss']
})
export class NotificationListItemComponent implements OnInit {
  @Input() notification: Notification;
  @Input() hideDelete = false;
  @Output() deleteRequested = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onDeleteRequested() {
    this.deleteRequested.emit();
  }

}
