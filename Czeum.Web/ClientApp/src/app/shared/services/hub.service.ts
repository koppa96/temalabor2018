import { Inject, Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { AuthService } from '../../authentication/services/auth.service';
import { API_BASE_URL } from '../clients';
import { Observable } from 'rxjs';
import { AuthState } from '../../reducers';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class HubService {
  private hubConnection: HubConnection;
  private authSate$: Observable<AuthState>;

  constructor(private authService: AuthService, @Inject(API_BASE_URL) private baseUrl: string) {
    this.authSate$ = this.authService.getAuthState();
  }

  connect(): Promise<void> {
    if (!this.hubConnection) {
      this.hubConnection = new HubConnectionBuilder()
        .withUrl(this.baseUrl + '/notifications', {
          accessTokenFactory: () => new Promise<string>((resolve, reject) => {
            this.authSate$.pipe(take(1)).subscribe(res => {
              console.log(res);
              if (res.isAuthenticated) {
                resolve(res.accessToken);
              } else {
                reject();
              }
            });
          })
        })
        .withAutomaticReconnect()
        .build();
    }

    if (this.hubConnection.state !== HubConnectionState.Connected) {
      return this.hubConnection.start();
    }

    return Promise.resolve();
  }

  disconnect(): Promise<void> {
    if (!this.hubConnection) {
      return Promise.resolve();
    }

    return this.hubConnection.stop();
  }

  registerCallback<T>(callbackId: string, callback: (param: T) => void) {
    this.hubConnection.on(callbackId, callback);
  }

  removeCallback<T>(callbackId: string, callback: (param: T) => void) {
    this.hubConnection.off(callbackId, callback);
  }
}

