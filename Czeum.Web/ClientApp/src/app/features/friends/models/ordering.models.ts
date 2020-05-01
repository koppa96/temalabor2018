import { Ordering } from 'src/app/shared/models/ordering.models';
import { FriendDto } from 'src/app/shared/clients';
import { firstBy } from 'thenby';

export const detailedFriendListOrderings: Array<Ordering<FriendDto>> = [
  {
    displayName: 'Legutóbb online',
    comparator: firstBy<FriendDto>(x => x.isOnline)
      .thenBy(x => x.lastDisconnect, 'desc')
  },
  {
    displayName: 'ABC sorrendben',
    comparator: firstBy<FriendDto>(x => x.username)
  }
];
