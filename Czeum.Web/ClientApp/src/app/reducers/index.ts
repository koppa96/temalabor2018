import {
  ActionReducer,
  ActionReducerMap,
  createFeatureSelector,
  createSelector,
  MetaReducer,
  createReducer
} from '@ngrx/store';
import { environment } from '../../environments/environment';
import { FriendListItem } from '../shared/models/friend-list.models';
import { friendListReducer } from './friend-list/friend-list-reducers';
import { soloQueueReducer } from './solo-queue/solo-queue-reducers';


export interface State {
  friendList: FriendListItem[];
  isQueueing: boolean;
}

export const reducers: ActionReducerMap<State> = {
  friendList: friendListReducer,
  isQueueing: soloQueueReducer
};


export const metaReducers: MetaReducer<State>[] = !environment.production ? [] : [];
