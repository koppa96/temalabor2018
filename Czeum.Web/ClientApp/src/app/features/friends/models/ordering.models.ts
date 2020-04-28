import { Ordering } from 'src/app/shared/models/ordering.models';
import { FriendDto } from 'src/app/shared/clients';
import { firstBy } from 'thenby';

export const detailedFriendListOrderings: Array<Ordering<FriendDto>> = [
  {
    displayName: 'ABC sorrendben',
    comparator: firstBy(x => x.username)
  },
  {
    displayName: 'Legutóbb online',
    comparator: firstBy(x => x.isOnline)
  }
];
