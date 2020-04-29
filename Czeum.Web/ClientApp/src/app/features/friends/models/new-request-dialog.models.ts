import { UserDto } from '../../../shared/clients';

export interface NewRequestDialogData {
  users: UserDto[];
}

export interface NewRequestDialogResult {
  selectedUser: UserDto;
}
