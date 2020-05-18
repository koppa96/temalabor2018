import { ActionReducerMap } from '@ngrx/store';
import { Profile } from '../authentication/auth-config-models';
import { LobbyDataWrapper, FriendDto } from '../shared/clients';
import { friendListReducerFunction } from './friend-list/friend-list-reducers';
import { soloQueueReducerFunction } from './solo-queue/solo-queue-reducers';
import { authStateReducerFunction, pkcsStringReducerFunction } from './authentication/auth-reducers';
import { currentLobbyReducerFunction } from './current-lobby/current-lobby-reducers';

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
  friendList: friendListReducerFunction,
  isQueueing: soloQueueReducerFunction,
  authState: authStateReducerFunction,
  pkceString: pkcsStringReducerFunction,
  currentLobby: currentLobbyReducerFunction
};
