import { Injectable } from '@angular/core';
import { FriendshipsClient, FriendDto } from '../clients';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FriendsService {

  constructor(private friendsClient: FriendshipsClient) { }

  getFriends(): Observable<FriendDto[]> {
    return this.friendsClient.getFriends();
  }

}
