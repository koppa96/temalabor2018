import { createAction, props } from '@ngrx/store';
import { NotificationDto } from '../../shared/clients';

export const updateNotificationList = createAction('[Notifications] UpdateNotificationList',
  props<{ notifications: NotificationDto[] }>());

export const newNotification = createAction('[Notifications] NewNotification',
  props<{ notification: NotificationDto }>());

export const deleteNotification = createAction('[Notifications] DeleteNotification',
  props<{ id: string }>());

export const deleteIncomingFriendRequestNotification = createAction('[Notifications] DeleteIncomingFriendRequestNotification',
  props<{ requestId: string }>());
