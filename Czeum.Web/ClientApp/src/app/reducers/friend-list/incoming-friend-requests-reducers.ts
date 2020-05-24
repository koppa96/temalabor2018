import { Action, createReducer, on } from '@ngrx/store';
import { FriendRequestDto } from '../../shared/clients';
import { addIncomingFriendRequest, removeIncomingFriendRequest, updateIncomingFriendRequestList } from './incoming-friend-requests-actions';

export const initialState: FriendRequestDto[] = [];

const incomingFriendRequestReducer = createReducer(initialState,
  on(updateIncomingFriendRequestList, (state, { requests }) => {
    return requests;
  }),
  on(addIncomingFriendRequest, (state, { request }) => {
    const newState = [...state];
    newState.splice(0, 0, request);
    return newState;
  }),
  on(removeIncomingFriendRequest, (state, { requestId }) => {
    return state.filter(x => x.id !== requestId);
  }));

export function incomingFriendRequestReducerFunction(state: FriendRequestDto[], action: Action) {
  return incomingFriendRequestReducer(state, action);
}
