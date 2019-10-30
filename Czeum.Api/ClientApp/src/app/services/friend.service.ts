import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FriendRequest } from '../models/friend-models';

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
}
