import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FriendRequest, Friendship } from '../models/friend-models';

@Injectable({
  providedIn: 'root'
})
export class FriendService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private apiUrl: string
  ) { }

  getReceivedRequests(): Observable<FriendRequest[]> {
    return this.http.get<FriendRequest[]>(this.apiUrl + 'api/friends/friend-requests/received');
  }

  getSentRequests(): Observable<FriendRequest[]> {
    return this.http.get<FriendRequest[]>(this.apiUrl + 'api/friends/friend-requests/sent');
  }

  sendFriendRequest(username: string): Observable<any> {
    return this.http.post(this.apiUrl + 'api/friends/friend-requests/' + username, undefined);
  }

  cancelRequest(requestId: string): Observable<any> {
    return this.http.delete(this.apiUrl + 'api/friends/friend-requests/' + requestId + '/cancel');
  }

  rejectRequest(requestId: string): Observable<any> {
    return this.http.delete(this.apiUrl + 'api/friends/friend-requests/' + requestId + '/reject');
  }

  getFriendships(): Observable<Friendship[]> {
    return this.http.get<Friendship[]>(this.apiUrl + 'api/friends/friendships');
  }

  deleteFriendship(friendshipId: string): Observable<any> {
    return this.http.delete(this.apiUrl + 'api/friends/friendships/' + friendshipId);
  }

  acceptRequest(requestId: string) {
    return this.http.post(this.apiUrl + 'api/friends/friendships/' + requestId, undefined);
  }
}
