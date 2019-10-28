import { UserInfo } from '../models/auth-models';

export interface AuthEventListener {
  onLoggedIn?(userInfo: UserInfo);
  onLoggedOut?();
}
