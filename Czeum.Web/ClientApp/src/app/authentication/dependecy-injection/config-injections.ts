import { InjectionToken } from '@angular/core';
import { ClientConfig, ServerConfig } from '../auth-config-models';
import { clientConfig, serverConfig } from '../auth-config';

export const CLIENT_CONFIG = new InjectionToken<ClientConfig>('ClientConfig', {
  providedIn: 'root',
  factory: () => clientConfig
});

export const SERVER_CONFIG = new InjectionToken<ServerConfig>('ServerConfig', {
  providedIn: 'root',
  factory: () => serverConfig
});
