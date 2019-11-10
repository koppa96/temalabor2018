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

export interface ChangePasswordData {
  oldPassword: string;
  password: string;
  confirmPassword: string;
}

export interface ResetPasswordData {
  username: string;
  password: string;
  token: string;
}

export interface ResetPasswordRequestData {
  username: string;
  email: string;
}

export interface ConfirmEmailData {
  username: string;
  token: string;
}
