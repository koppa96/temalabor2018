import { createReducer, on } from '@ngrx/store';
import { updateLobby, leaveLobby } from './current-lobby-actions';
import { LobbyDataWrapper } from '../../shared/clients';

export const currentLobbyReducer = createReducer<LobbyDataWrapper | null>(null,
  on(updateLobby, (state, { newLobby }) => {
    return newLobby;
  }),
  on(leaveLobby, state => {
    return null;
  }));
