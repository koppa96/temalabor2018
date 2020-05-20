import { Injectable } from '@angular/core';
import { NotificationDto, NotificationsClient } from '../clients';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  constructor(private client: NotificationsClient) { }

  getNotifications(): Observable<NotificationDto[]> {
    return this.client.getNotifications();
  }

  deleteNotification(id: string): Observable<void> {
    return this.client.deleteNotification(id);
  }
}
