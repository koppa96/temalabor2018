import { createReducer, on } from '@ngrx/store';
import { AuthState } from '../index';
import { updateAuthState, updatePkceString } from './auth-actions';

export const initialState: AuthState = {
  isAuthenticated: false,
  accessToken: null,
  idToken: null,
  profile: null,
  expires: null
};

export const initialPkceState = '';

export const authStateReducer = createReducer(initialState,
  on(updateAuthState, (state, { updatedState }) => {
    return updatedState;
  }));

export const pkcsStringReducer = createReducer(initialPkceState,
  on(updatePkceString, (state, { updatedString }) => {
    return updatedString;
  }));
