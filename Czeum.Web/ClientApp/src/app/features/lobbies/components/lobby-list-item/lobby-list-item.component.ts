import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { GameTypeDto, LobbyAccess, LobbyDataWrapper } from '../../../../shared/clients';
import { IconDefinition } from '@fortawesome/fontawesome-common-types';
import { GameIconMapperService } from '../../../games/services/game-icon-mapper.service';
import { lobbyAccessDropdownItems } from '../../models/lobby-create.models';
import { FriendListItem } from '../../../../shared/models/friend-list.models';

@Component({
  selector: 'app-lobby-list-item',
  templateUrl: './lobby-list-item.component.html',
  styleUrls: ['./lobby-list-item.component.scss']
})
export class LobbyListItemComponent implements OnInit {
  @Input() lobby: LobbyDataWrapper;
  @Input() gameTypes: GameTypeDto[] = [];
  @Input() currentUserName: string;
  @Input() friendList: FriendListItem[];

  @Output() joinLobby = new EventEmitter<string>();
  @Output() leaveCurrentLobby = new EventEmitter();

  lobbyAccesses = lobbyAccessDropdownItems;

  constructor(private gameIconMapperService: GameIconMapperService) { }

  ngOnInit() {
  }

  getIcon(gameIdentifier: number): IconDefinition {
    return this.gameIconMapperService.mapIcon(gameIdentifier);
  }

  getGameTypeDisplayName(): string {
    const gameType = this.gameTypes.find(x => x.identifier === this.lobby.gameIdentifier);
    if (gameType) {
      return gameType.displayName;
    } else {
      return '';
    }
  }

  getLobbyAccessDisplayName(): string {
    return this.lobbyAccesses.find(x => x.lobbyAccess === this.lobby.content.access).displayName;
  }

  canJoin(lobby: LobbyDataWrapper): boolean {
    return lobby.content.host !== this.currentUserName && !lobby.content.guests.includes(this.currentUserName) &&
      (lobby.content.access === LobbyAccess.Public ||
      lobby.content.access === LobbyAccess.FriendsOnly && this.friendList.some(x => x.username === lobby.content.host) ||
      lobby.content.invitedPlayers.includes(this.currentUserName));
  }

  isInvited(lobby: LobbyDataWrapper): boolean {
    return lobby.content.invitedPlayers.includes(this.currentUserName);
  }

  onJoinClicked(lobbyId: string) {
    this.joinLobby.emit(lobbyId);
  }

  isCurrentLobby(lobby: LobbyDataWrapper): boolean {
    return lobby.content.host === this.currentUserName || lobby.content.guests.includes(this.currentUserName);
  }

  onLeaveClicked() {
    this.leaveCurrentLobby.emit();
  }
}
