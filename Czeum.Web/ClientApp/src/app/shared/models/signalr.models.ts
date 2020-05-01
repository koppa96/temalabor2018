import { Message } from '../clients';

export interface MessageReceivedArgs {
  id: string;
  message: Message;
}
