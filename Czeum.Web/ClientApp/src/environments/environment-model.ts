export interface ClientConfig {
  clientId: string;
  scope: string;
  responseType: string;
  postLoginRedirectUri: string;
  silentRefreshRedirectUri: string;
  postLogoutRedirectUri: string;
}

export interface ServerConfig {
  authorizeUrl: string;
  endsessionUrl: string;
  tokenUrl: string;
}

export interface Environment {
  production: boolean;
  apiBaseUrl: string;
  authConfig: { clientConfig: ClientConfig; serverConfig: ServerConfig; };
}
