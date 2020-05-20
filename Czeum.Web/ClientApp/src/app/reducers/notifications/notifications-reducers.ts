import { Action, createReducer, on } from '@ngrx/store';
import { NotificationDto } from '../../shared/clients';
import { deleteNotification, newNotification, updateNotificationList } from './notifications-actions';

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
  }));

export function notificationsReducerFunction(state: NotificationDto[], action: Action) {
  return notificationsReducer(state, action);
}
