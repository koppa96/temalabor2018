import {createAction, props} from '@ngrx/store';
import {FriendListItem} from '../../shared/models/friend-list.models';

export const joinSoloQueue = createAction('[SoloQueue] Join');
export const leaveSoloQueue = createAction('[SoloQueue] Leave');
