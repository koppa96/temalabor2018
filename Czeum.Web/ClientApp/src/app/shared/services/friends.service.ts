import { Injectable } from '@angular/core';
import { FriendshipsClient, FriendDto, FriendRequestDto, FriendRequestsClient, UserDto, AccountsClient } from '../clients';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { toLocalDate } from './date-utils';

@Injectable({
  providedIn: 'root'
})
export class FriendsService {

  constructor(
    private friendsClient: FriendshipsClient,
    private friendRequestsClient: FriendRequestsClient,
    private accountsClient: AccountsClient
  ) { }

  getFriends(): Observable<FriendDto[]> {
    return this.friendsClient.getFriends().pipe(
      tap(dtos => {
        for (const dto of dtos) {
          dto.lastDisconnect = toLocalDate(dto.lastDisconnect);
          dto.registrationTime = toLocalDate(dto.registrationTime);
        }
      })
    );
  }

  getOutgoingFriendRequests(): Observable<FriendRequestDto[]> {
    return this.friendRequestsClient.getFriendRequestsSent().pipe(
      tap(dtos => {
        for (const dto of dtos) {
          dto.sentAt = toLocalDate(dto.sentAt);
        }
      })
    );
  }

  getIncomingFriendRequests(): Observable<FriendRequestDto[]> {
    return this.friendRequestsClient.getFriendRequestsReceived().pipe(
      tap(dtos => {
        for (const dto of dtos) {
          dto.sentAt = toLocalDate(dto.sentAt);
        }
      })
    );
  }

  sendFriendRequest(userId: string): Observable<FriendRequestDto> {
    return this.friendRequestsClient.sendFriendRequest(userId);
  }

  cancelFriendRequest(requestId: string): Observable<void> {
    return this.friendRequestsClient.cancelFriendRequest(requestId);
  }

  getUsernameAutocomplete(searchText?: string): Observable<UserDto[]> {
    return this.accountsClient.getUsernames(searchText || '');
  }

  acceptRequest(requestId: string): Observable<FriendDto> {
    return this.friendsClient.acceptRequest(requestId);
  }

  rejectRequest(requestId: string): Observable<void> {
    return this.friendRequestsClient.rejectFriendRequest(requestId);
  }

  removeFriend(friendshipId: string) {
    return this.friendsClient.removeFriend(friendshipId);
  }
}
