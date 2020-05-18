import { InjectionToken } from '@angular/core';
import { ClientConfig, ServerConfig } from '../auth-config-models';
import { environment } from '../../../environments/environment';

export const CLIENT_CONFIG = new InjectionToken<ClientConfig>('ClientConfig', {
  providedIn: 'root',
  factory: () => environment.authConfig.clientConfig
});

export const SERVER_CONFIG = new InjectionToken<ServerConfig>('ServerConfig', {
  providedIn: 'root',
  factory: () => environment.authConfig.serverConfig
});
