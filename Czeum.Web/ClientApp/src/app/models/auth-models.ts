export interface LoginData {
  username: string;
  password: string;
}

export interface RegisterData {
  username: string;
  email: string;
  password: string;
}

export interface UserInfo {
  userId: string;
  username: string;
  email: string;
}

export interface TokenResponse {
  access_token: string;
  refresh_token: string;
  token_type: string;
}
