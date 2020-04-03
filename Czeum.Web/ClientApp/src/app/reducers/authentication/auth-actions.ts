import { createAction, props } from '@ngrx/store';
import { AuthState } from '../index';

export const updateAuthState = createAction('[AuthState] Update',
  props<{ updatedState: AuthState; }>());

export const updatePkceString = createAction('[PkceString] updatePkceString',
  props<{ updatedString: string; }>());
