import { Action, createReducer, on } from '@ngrx/store';
import { joinSoloQueue, leaveSoloQueue } from './solo-queue-actions';

export const initialState = false;

const soloQueueReducer = createReducer(initialState,
  on(joinSoloQueue, state => true),
  on(leaveSoloQueue, state => false));

export function soloQueueReducerFunction(state: boolean, action: Action) {
  return soloQueueReducer(state, action);
}
