import { createReducer, on } from '@ngrx/store';
import { FriendListItem } from '../../shared/models/friend-list.models';
import { updateFriendList } from './friend-list-actions';

export const initialState: FriendListItem[] = [
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: false,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  },
  {
    id: '1',
    username: 'gipsz.jakab',
    isOnline: true,
    imageUrl: 'https://images-na.ssl-images-amazon.com/images/I/81-yKbVND-L._SY355_.png',
    isInvited: false
  }
];

export const friendListReducer = createReducer(initialState,
  on(updateFriendList, (state, { updatedList }) => {
    console.log(updatedList);
    return state;
  }));
