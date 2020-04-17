import { createAction, props } from '@ngrx/store';
import { LobbyDataWrapper } from '../../shared/clients';

export const updateLobby = createAction('[Current Lobby] Join',
  props<{ newLobby: LobbyDataWrapper }>());

export const leaveLobby = createAction('[Current Lobby] Left');
