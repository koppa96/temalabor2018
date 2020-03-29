import { ClientConfig, ServerConfig } from './auth-config-models';

export const clientConfig: ClientConfig = {
  clientId: 'czeum_angular_client',
  postLoginRedirectUri: 'http://localhost:4200/signin-oidc',
  postLogoutRedirectUri: 'http://localhost:4200/signout-oidc',
  responseType: 'code',
  scope: 'openid profile czeum_api'
};

export const serverConfig: ServerConfig = {
  authorizeUrl: 'https://localhost:5001/connect/authorize',
  endsessionUrl: 'https://localhost:5001/connect/endsession',
  tokenUrl: 'https://localhost:5001/connect/token'
};
