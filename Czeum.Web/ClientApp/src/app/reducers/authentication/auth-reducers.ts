import { Action, createReducer, on } from '@ngrx/store';
import { AuthState } from '../index';
import { updateAuthState, updateIsHandling, updatePkceString } from './auth-actions';

export const initialState: AuthState = {
  isAuthenticated: false,
  accessToken: null,
  idToken: null,
  profile: null,
  expires: null,
  isHandling: false
};

export const initialPkceState = '';

const authStateReducer = createReducer(initialState,
  on(updateAuthState, (state, { updatedState }) => {
    return updatedState;
  }),
  on(updateIsHandling, (state, { isHandling }) => {
    return {
      isAuthenticated: state.isAuthenticated,
      accessToken: state.accessToken,
      idToken: state.idToken,
      profile: state.profile,
      expires: state.expires,
      isHandling
    };
  }));

const pkcsStringReducer = createReducer(initialPkceState,
  on(updatePkceString, (state, { updatedString }) => {
    return updatedString;
  }));

export function authStateReducerFunction(state: AuthState, action: Action) {
  return authStateReducer(state, action);
}

export function pkcsStringReducerFunction(state: string, action: Action) {
  return pkcsStringReducer(state, action);
}
