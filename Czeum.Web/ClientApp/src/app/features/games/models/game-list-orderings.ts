import { Ordering } from '../../../shared/models/ordering.models';
import { MatchStatus } from '../../../shared/clients';
import { firstBy } from 'thenby';

export const gameListOrderings: Ordering<MatchStatus>[] = [
  {
    displayName: 'Legújabb elöl',
    comparator: firstBy<MatchStatus>(x => x.lastMoveDate, 'desc')
  },
  {
    displayName: 'Legrégebbi elöl',
    comparator: firstBy<MatchStatus>(x => x.lastMoveDate)
  },
  {
    displayName: 'Játéktípus szerint',
    comparator: firstBy<MatchStatus>(x => x.currentBoard.gameIdentifier)
  }
];
