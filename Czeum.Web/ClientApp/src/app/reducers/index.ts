import { ActionReducerMap } from '@ngrx/store';
import { friendListReducer } from './friend-list/friend-list-reducers';
import { soloQueueReducer } from './solo-queue/solo-queue-reducers';
import { Profile } from '../authentication/auth-config-models';
import { authStateReducer, pkcsStringReducer } from './authentication/auth-reducers';
import { LobbyDataWrapper, FriendDto } from '../shared/clients';
import { currentLobbyReducer } from './current-lobby/current-lobby-reducers';

export interface AuthState {
  isAuthenticated: boolean;
  profile: Profile | null;
  accessToken: string | null;
  idToken: string | null;
  expires: Date | null;
  isHandling: boolean;
}

export interface State {
  friendList: FriendDto[];
  isQueueing: boolean;
  authState: AuthState;
  pkceString: string;
  currentLobby: LobbyDataWrapper;
}

export const reducers: ActionReducerMap<State> = {
  friendList: friendListReducer,
  isQueueing: soloQueueReducer,
  authState: authStateReducer,
  pkceString: pkcsStringReducer,
  currentLobby: currentLobbyReducer
};
