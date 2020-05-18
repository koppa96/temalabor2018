import { Environment } from './environment-model';

export const environment: Environment = {
  production: true,
  apiBaseUrl: 'https://czeum.azurewebsites.net',
  authConfig: {
    clientConfig: {
      clientId: 'czeum_angular_client',
      postLoginRedirectUri: 'https://czeum.azurewebsites.net/signin-oidc',
      silentRefreshRedirectUri: 'https://czeum.azurewebsites.net/silent-refresh.html',
      postLogoutRedirectUri: 'https://czeum.azurewebsites.net/signout-oidc',
      responseType: 'code',
      scope: 'openid profile czeum_api'
    },
    serverConfig: {
      authorizeUrl: 'https://czeum.azurewebsites.net/connect/authorize',
      endsessionUrl: 'https://czeum.azurewebsites.net/connect/endsession',
      tokenUrl: 'https://czeum.azurewebsites.net/connect/token'
    }
  }
};
