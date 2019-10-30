export interface Friendship {
  friendshipId: string;
  isOnline: boolean;
  username: string;
}

export interface FriendRequest {
  id: string;
  senderName: string;
  receiverName: string;
}
