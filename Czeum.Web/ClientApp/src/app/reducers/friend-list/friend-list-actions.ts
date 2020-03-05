import { createAction, props } from '@ngrx/store';
import { FriendListItem } from '../../shared/models/friend-list.models';

export const updateFriendList = createAction('[SharedFriendList] Update',
  props<{ updatedList: FriendListItem[]; }>());
