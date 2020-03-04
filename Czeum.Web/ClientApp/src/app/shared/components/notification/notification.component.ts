import { Component, OnInit } from '@angular/core';
import { Notification, mockNotifications } from '../../models/notification.models';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent implements OnInit {
  notifications: Notification[] = mockNotifications;

  constructor() { }

  ngOnInit() {
  }

  deleteRequested(notification: Notification) {
    alert('Ez veszélyes!');
  }

}
