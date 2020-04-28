import { createReducer, on } from '@ngrx/store';
import { updateFriendList, updateFriend, addFriend, removeFriend } from './friend-list-actions';
import { FriendDto } from 'src/app/shared/clients';
import { firstBy } from 'thenby';

export const initialState: FriendDto[] = [];

export const friendListReducer = createReducer(initialState,
  on(updateFriendList, (state, { updatedList }) => {
    return updatedList;
  }),
  on(updateFriend, (state, { friend }) => {
    const newState: FriendDto[] = [];
    for (const element of state) {
      if (element.friendshipId !== friend.friendshipId) {
        newState.push(element);
      } else {
        newState.push(friend);
      }
    }

    return newState.sort(firstBy<FriendDto>(x => x.isOnline)
      .thenBy<FriendDto>(x => x.username));
  }),
  on(addFriend, (state, { friend }) => {
    const newState = [...state, friend];
    return newState.sort(firstBy<FriendDto>(x => x.isOnline)
      .thenBy<FriendDto>(x => x.username));
  }),
  on((removeFriend), (state, { friendshipId }) => {
    return state.filter(x => x.friendshipId !== friendshipId);
  }));
