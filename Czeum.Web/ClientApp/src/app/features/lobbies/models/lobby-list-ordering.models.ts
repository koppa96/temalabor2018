import { LobbyDataWrapper } from '../../../shared/clients';

export interface LobbyListOrdering {
  displayName: string;
  comparator: (left: LobbyDataWrapper, right: LobbyDataWrapper) => number;
}

export const lobbyListOrderings: LobbyListOrdering[] = [
  {
    displayName: 'Legújabb elöl',
    comparator: (left, right) => {
      if (left.content.created > right.content.created) {
        return -1;
      } else if (right.content.created > left.content.created) {
        return 1;
      } else {
        return 0;
      }
    }
  },
  {
    displayName: 'Legrégebbi elöl',
    comparator: (left, right) => {
      if (left.content.created < right.content.created) {
        return -1;
      } else if (right.content.created < left.content.created) {
        return 1;
      } else {
        return 0;
      }
    }
  },
  {
    displayName: 'ABC sorrendben',
    comparator: (left, right) => {
      const leftName = left.content.name || `${left.content.host} szobája`;
      const rightName = right.content.name || `${right.content.host} szobája`;
      if (leftName < rightName) {
        return -1;
      } else if (rightName < leftName) {
        return 1;
      } else {
        return 0;
      }
    }
  }
];
