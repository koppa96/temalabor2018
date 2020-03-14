import { createReducer, on } from '@ngrx/store';
import { joinSoloQueue, leaveSoloQueue } from './solo-queue-actions';

export const initialState = false;

export const soloQueueReducer = createReducer(initialState,
  on(joinSoloQueue, state => true),
  on(leaveSoloQueue, state => false));
