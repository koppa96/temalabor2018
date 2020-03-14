import { Component, OnInit } from '@angular/core';
import { Notification, mockNotifications } from '../../models/notification.models';
import { Store } from '@ngrx/store';
import { State } from '../../../reducers';
import { Observable } from 'rxjs';
import { leaveSoloQueue } from '../../../reducers/solo-queue/solo-queue-actions';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.scss']
})
export class NotificationComponent implements OnInit {
  isQueuing: Observable<boolean>;
  notifications: Notification[] = mockNotifications;

  queueNotification: Notification = new Notification({
    icon: 'speed',
    text: 'Gyors játék keresés folyamatban...',
    actions: {
      primary: {
        name: 'LEÁLLÍTÁS',
        action: () => this.store.dispatch(leaveSoloQueue())
      }
    }
  });

  constructor(private store: Store<State>) {
    this.isQueuing = this.store.select(x => x.isQueueing);
  }

  ngOnInit() {

  }

  deleteRequested(notification: Notification) {
    alert('Ez veszélyes!');
  }

}
