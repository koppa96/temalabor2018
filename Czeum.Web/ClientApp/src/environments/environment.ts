// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { Environment } from './environment-model';

export const environment: Environment = {
  production: false,
  apiBaseUrl: 'https://localhost:5001',
  authConfig: {
    clientConfig: {
      clientId: 'czeum_angular_client',
      postLoginRedirectUri: 'http://localhost:4200/signin-oidc',
      silentRefreshRedirectUri: 'http://localhost:4200/silent-refresh.html',
      postLogoutRedirectUri: 'http://localhost:4200/signout-oidc',
      responseType: 'code',
      scope: 'openid profile czeum_api'
    },
    serverConfig: {
      authorizeUrl: 'https://localhost:5001/connect/authorize',
      endsessionUrl: 'https://localhost:5001/connect/endsession',
      tokenUrl: 'https://localhost:5001/connect/token'
    }
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
