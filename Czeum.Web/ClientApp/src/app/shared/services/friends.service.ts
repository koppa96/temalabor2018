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
    return this.friendsClient.getFriends();
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

  sendFriendRequest(userId: string): Observable<FriendRequestDto> {
    return this.friendRequestsClient.sendFriendRequest(userId);
  }

  cancelFriendRequest(requestId: string): Observable<void> {
    return this.friendRequestsClient.cancelFriendRequest(requestId);
  }

  getUsernameAutocomplete(searchText?: string): Observable<UserDto[]> {
    return this.accountsClient.getUsernames(searchText || '');
  }

  cancelRequest(requestId: string): Observable<void> {
    return this.friendRequestsClient.cancelFriendRequest(requestId);
  }

}
