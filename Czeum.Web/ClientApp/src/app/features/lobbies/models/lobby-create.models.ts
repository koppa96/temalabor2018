import { GameTypeDto, LobbyAccess } from '../../../shared/clients';

export interface LobbyAccessDropdownItem {
  lobbyAccess: LobbyAccess;
  displayName: string;
}

export interface LobbyCreateDetails {
  lobbyAccess: LobbyAccessDropdownItem;
  gameType: GameTypeDto;
  name: string | null;
}

export const lobbyAccessDropdownItems: LobbyAccessDropdownItem[] = [
  {
    lobbyAccess: LobbyAccess.Public,
    displayName: 'Mindenki'
  },
  {
    lobbyAccess: LobbyAccess.FriendsOnly,
    displayName: 'Barátok és meghívottak'
  },
  {
    lobbyAccess: LobbyAccess.Private,
    displayName: 'Csak meghívottak'
  }
];
