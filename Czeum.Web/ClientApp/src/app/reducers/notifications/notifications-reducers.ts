import { Action, createReducer, on } from '@ngrx/store';
import { NotificationDto, NotificationType } from '../../shared/clients';
import {
  deleteIncomingFriendRequestNotification,
  deleteNotification,
  newNotification,
  updateNotificationList
} from './notifications-actions';

export const initialState: NotificationDto[] = [];

const notificationsReducer = createReducer(initialState,
  on(updateNotificationList, (state, { notifications }) => {
    return notifications;
  }),
  on(newNotification, (state, { notification }) => {
    return state.concat(notification);
  }),
  on(deleteNotification, (state, { id }) => {
    return state.filter(x => x.id !== id);
  }),
  on(deleteIncomingFriendRequestNotification, (state, { requestId }) => {
    return state.filter(x => !(x.type === NotificationType.FriendRequestReceived && x.data === requestId));
  }));

export function notificationsReducerFunction(state: NotificationDto[], action: Action) {
  return notificationsReducer(state, action);
}
