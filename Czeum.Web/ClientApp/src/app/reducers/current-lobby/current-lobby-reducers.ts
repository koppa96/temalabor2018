import { Action, createReducer, on } from '@ngrx/store';
import { updateLobby, leaveLobby } from './current-lobby-actions';
import { LobbyDataWrapper } from '../../shared/clients';

const currentLobbyReducer = createReducer<LobbyDataWrapper | null>(null,
  on(updateLobby, (state, { newLobby }) => {
    return newLobby;
  }),
  on(leaveLobby, state => {
    return null;
  }));

export function currentLobbyReducerFunction(state: LobbyDataWrapper, action: Action) {
  return currentLobbyReducer(state, action);
}
