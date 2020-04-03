export interface FriendListItem {
  id: string;
  imageUrl: string;
  username: string;
  isOnline: boolean;
  isInvited: boolean;
  lastDisconnect: Date;
  registrationTime: Date;
}
