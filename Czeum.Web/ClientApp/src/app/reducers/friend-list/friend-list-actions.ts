import { createAction, props } from '@ngrx/store';
import { FriendDto } from 'src/app/shared/clients';

export const updateFriendList = createAction('[SharedFriendList] Update',
  props<{ updatedList: FriendDto[]; }>());

export const updateFriend = createAction('[SharedFriendList] UpdateFriend',
  props<{ friend: FriendDto }>());

export const addFriend = createAction('[SharedFriendList] AddFriend',
  props<{ friend: FriendDto }>());

export const removeFriend = createAction('[SharedFriendList] RemoveFriend',
  props<{ friendshipId: string }>());
