import { Injectable } from '@angular/core';
import { HubService } from './hub.service';
import { Subject } from 'rxjs';
import { AchivementDto, FriendDto, FriendRequestDto, LobbyDataWrapper, MatchStatus, Message, NotificationDto } from '../clients';
import { MessageReceivedArgs } from '../models/signalr.models';

@Injectable({
  providedIn: 'root'
})
export class ObservableHub {
  // Games
  private mReceiveResult = new Subject<MatchStatus>();
  private mMatchCreated = new Subject<MatchStatus>();
  private mAchivementUnlocked = new Subject<AchivementDto>();

  // Lobbies
  private mLobbyDeleted = new Subject<string>();
  private mLobbyAdded = new Subject<LobbyDataWrapper>();
  private mLobbyChanged = new Subject<LobbyDataWrapper>();
  private mKickedFromLobby = new Subject();
  private mReceiveLobbyInvite = new Subject<LobbyDataWrapper>();

  // Friends
  private mReceiveRequest = new Subject<FriendRequestDto>();
  private mRequestRejected = new Subject<string>();
  private mRequestRevoked = new Subject<string>();
  private mFriendAdded = new Subject<FriendDto>();
  private mFriendRemoved = new Subject<string>();
  private mFriendConnectionStateChanged = new Subject<FriendDto>();

  // Messages
  private mReceiveDirectMessage = new Subject<MessageReceivedArgs>();
  private mReceiveMatchMessage = new Subject<MessageReceivedArgs>();
  private mReceiveLobbyMessage = new Subject<MessageReceivedArgs>();

  private mNotificationReceived = new Subject<NotificationDto>();

  // --- Observables ---
  // Games
  get receiveResult() { return this.mReceiveResult.asObservable(); }
  get matchCreated() { return this.mMatchCreated.asObservable(); }
  get achivementUnlocked() { return this.mAchivementUnlocked.asObservable(); }

  // Lobbies
  get lobbyDeleted() { return this.mLobbyDeleted.asObservable(); }
  get lobbyAdded() { return this.mLobbyAdded.asObservable(); }
  get lobbyChanged() { return this.mLobbyChanged.asObservable(); }
  get kickedFromLobby() { return this.mKickedFromLobby.asObservable(); }
  get receiveLobbyInvite() { return this.mReceiveLobbyInvite.asObservable(); }

  // Friends
  get receiveRequest() { return this.mReceiveRequest.asObservable(); }
  get requestRejected() { return this.mRequestRejected.asObservable(); }
  get requestRevoked() { return this.mRequestRevoked.asObservable(); }
  get friendAdded() { return this.mFriendAdded.asObservable(); }
  get friendRemoved() { return this.mFriendRemoved.asObservable(); }
  get friendConnectionStateChanged() { return this.mFriendConnectionStateChanged.asObservable(); }

  // Messages
  get receiveDirectMessage() { return this.mReceiveDirectMessage.asObservable(); }
  get receiveMatchMessage() { return this.mReceiveMatchMessage.asObservable(); }
  get receiveLobbyMessage() { return this.mReceiveLobbyMessage.asObservable(); }

  get notificationReceived() { return this.mNotificationReceived.asObservable(); }

  constructor(private hubService: HubService) {}

  async createConnection(): Promise<void> {
    await this.hubService.connect();

    this.hubService.registerCallback('ReceiveResult', (status: MatchStatus) => this.mReceiveResult.next(status));
    this.hubService.registerCallback('MatchCreated', (status: MatchStatus) => this.mMatchCreated.next(status));
    this.hubService.registerCallback('AchivementUnlocked', (achivement: AchivementDto) => this.mAchivementUnlocked.next(achivement));

    this.hubService.registerCallback('LobbyDeleted', (lobbyId: string) => this.mLobbyDeleted.next(lobbyId));
    this.hubService.registerCallback('LobbyAdded', (lobby: LobbyDataWrapper) => this.mLobbyAdded.next(lobby));
    this.hubService.registerCallback('LobbyChanged', (lobby: LobbyDataWrapper) => this.mLobbyChanged.next(lobby));
    this.hubService.registerCallback('KickedFromLobby', () => this.mKickedFromLobby.next());
    this.hubService.registerCallback('ReceiveLobbyInvite', (lobby: LobbyDataWrapper) => this.mReceiveLobbyInvite.next(lobby));

    this.hubService.registerCallback('ReceiveRequest', (request: FriendRequestDto) => this.mReceiveRequest.next(request));
    this.hubService.registerCallback('RequestRejected', (requestId: string) => this.mRequestRejected.next(requestId));
    this.hubService.registerCallback('RequestRevoked', (requestId: string) => this.mRequestRevoked.next(requestId));
    this.hubService.registerCallback('FriendAdded', (friend: FriendDto) => this.mFriendAdded.next(friend));
    this.hubService.registerCallback('FriendRemoved', (friendshipId: string) => this.mFriendRemoved.next(friendshipId));
    this.hubService.registerCallback('FriendConnectionStateChanged',
      (friend: FriendDto) => this.mFriendConnectionStateChanged.next(friend));

    this.hubService.registerCallback('ReceiveDirectMessage', (friendshipId: string, message: Message) => {
      this.mReceiveDirectMessage.next({
        id: friendshipId,
        message
      });
    });

    this.hubService.registerCallback('ReceiveMatchMessage', (matchId: string, message: Message) => {
      this.mReceiveMatchMessage.next({
        id: matchId,
        message
      });
    });

    this.hubService.registerCallback('ReceiveLobbyMessage', (lobbyId: string, message: Message) => {
      this.mReceiveLobbyMessage.next({
        id: lobbyId,
        message
      });
    });

    this.hubService.registerCallback('NotificationReceived', (notification: NotificationDto) => {
      this.mNotificationReceived.next(notification);
    });
  }

  disconnect(): Promise<void> {
    this.hubService.removeCallback('ReceiveResult');
    this.hubService.removeCallback('MatchCreated');
    this.hubService.removeCallback('AchivementUnlocked');

    this.hubService.removeCallback('LobbyDeleted');
    this.hubService.removeCallback('LobbyAdded');
    this.hubService.removeCallback('LobbyChanged');
    this.hubService.removeCallback('KickedFromLobby');
    this.hubService.removeCallback('ReceiveLobbyInvite');

    this.hubService.removeCallback('ReceiveRequest');
    this.hubService.removeCallback('RequestRejected');
    this.hubService.removeCallback('RequestRevoked');
    this.hubService.removeCallback('FriendAdded');
    this.hubService.removeCallback('FriendRemoved');
    this.hubService.removeCallback('FriendConnectionStateChanged');

    this.hubService.removeCallback('ReceiveDirectMessage');
    this.hubService.removeCallback('ReceiveMatchMessage');
    this.hubService.removeCallback('ReceiveLobbyMessage');

    this.hubService.removeCallback('NotificationReceived');

    return this.hubService.disconnect();
  }
}
