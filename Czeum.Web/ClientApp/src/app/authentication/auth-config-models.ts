export interface ClientConfig {
  clientId: string;
  scope: string;
  responseType: string;
  postLoginRedirectUri: string;
postLogoutRedirectUri: string;
}

export interface ServerConfig {
  authorizeUrl: string;
  endsessionUrl: string;
  tokenUrl: string;
}

export interface Profile {
  userId: string;
  userName: string;
}
