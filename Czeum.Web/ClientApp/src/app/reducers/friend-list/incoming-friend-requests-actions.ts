import { createAction, props } from '@ngrx/store';
import { FriendRequestDto } from '../../shared/clients';

export const updateIncomingFriendRequestList = createAction('[IncomingFriendRequestList] Update',
  props<{ requests: FriendRequestDto[] }>());

export const addIncomingFriendRequest = createAction('[IncomingFriendRequestList] Add',
  props<{ request: FriendRequestDto }>());

export const removeIncomingFriendRequest = createAction('[IncomingFriendRequestList] Remove',
  props<{ requestId: string }>());
