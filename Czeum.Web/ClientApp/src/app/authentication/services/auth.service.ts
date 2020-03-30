import { Inject, Injectable } from '@angular/core';
import { ClientConfig, ServerConfig } from '../auth-config-models';
import { Store } from '@ngrx/store';
import { AuthState, State } from '../../reducers';
import { take, tap } from 'rxjs/operators';
import { updateAuthState, updatePkceString } from '../../reducers/authentication/auth-actions';
import { CLIENT_CONFIG, SERVER_CONFIG } from '../dependecy-injection/config-injections';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { create } from 'pkce';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Check if authService is currently handling an authentication request
  static isHandling = false;
  private authListeners: Array<() => void> = [];

  constructor(
    @Inject(CLIENT_CONFIG) private clientConfig: ClientConfig,
    @Inject(SERVER_CONFIG) private serverConfig: ServerConfig,
    private store: Store<State>,
    private http: HttpClient,
    private router: Router) {
    document.addEventListener('silent-refresh-callback', event => {
      this.silentRefreshCallback((event as CustomEvent).detail);
    });
  }

  private createAuthorizeUrl(silentRefresh: boolean) {
    const codePair = create();
    this.store.dispatch(updatePkceString({ updatedString: codePair.codeVerifier }));

    const params = new URLSearchParams();
    params.append('client_id', this.clientConfig.clientId);
    params.append('scope', this.clientConfig.scope);
    params.append('response_type', this.clientConfig.responseType);

    params.append('code_challenge', codePair.codeChallenge);
    params.append('code_challenge_method', 'S256');
    if (silentRefresh) {
      params.append('prompt', 'none');
      params.append('redirect_uri', this.clientConfig.silentRefreshRedirectUri);
    } else {
      params.append('redirect_uri', this.clientConfig.postLoginRedirectUri);
    }

    return `${this.serverConfig.authorizeUrl}?${params.toString()}`;
  }

  addAuthenticationCallback(callback: () => void) {
    this.authListeners.push(callback);
  }

  initiateAuthCodeFlow() {
    AuthService.isHandling = true;
    window.location.href = this.createAuthorizeUrl(false);
  }

  onAuthCodeReceived(authCode: string, silentRenew: boolean): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.store.select(x => x.pkceString).pipe(
        take(1)
      ).subscribe(res => {
        const params = new URLSearchParams();
        params.append('client_id', this.clientConfig.clientId);
        params.append('scope', this.clientConfig.scope);
        params.append('grant_type', 'authorization_code');
        params.append('code', authCode);

        params.append('code_verifier', res);

        if (silentRenew) {
          params.append('redirect_uri', this.clientConfig.silentRefreshRedirectUri);
        } else {
          params.append('redirect_uri', this.clientConfig.postLoginRedirectUri);
        }

        this.http.post(this.serverConfig.tokenUrl, params.toString(), {
          headers: new HttpHeaders({
            'Content-Type': 'application/x-www-form-urlencoded'
          })
        }).subscribe(
          (tokenResponse: { id_token: string; access_token: string; }) => {
            this.onPostLogin(tokenResponse.id_token, tokenResponse.access_token);
            AuthService.isHandling = false;
            this.authListeners.forEach(x => x());
            resolve();
          },
          () => {
            AuthService.isHandling = false;
            this.authListeners.forEach(x => x());
            reject();
          }
        );
      });
    });
  }

  initiateLogout() {
    this.store.select(x => x.authState).pipe(
      take(1)
    ).subscribe(res => {
      if (res.isAuthenticated) {
        const params = new URLSearchParams();
        params.append('id_token_hint', res.idToken);
        params.append('post_logout_redirect_uri', this.clientConfig.postLogoutRedirectUri);
        window.location.href = `${this.serverConfig.endsessionUrl}?${params.toString()}`;
      }
    });
  }

  onPostLogin(idToken: string, accessToken: string) {
    const tokenPayload = JSON.parse(atob(accessToken.split('.')[1]));

    const authState: AuthState = {
      idToken,
      accessToken,
      isAuthenticated: true,
      profile: {
        userId: tokenPayload.sub,
        userName: tokenPayload.name
      },
      expires: new Date(tokenPayload.exp * 1000)
    };

    this.store.dispatch(updateAuthState({updatedState: authState}));
  }

  onPostLogout() {
    const authState: AuthState = {
      idToken: null,
      accessToken: null,
      profile: null,
      isAuthenticated: false,
      expires: null
    };

    this.store.dispatch(updateAuthState({updatedState: authState}));
  }

  getAuthState(): Observable<AuthState> {
    return this.store.select(x => x.authState).pipe(
      tap(x => {
        if (typeof x.expires === 'string') {
          x.expires = new Date(x.expires);
        }
      })
    );
  }

  silentRefreshIfRequired(): Promise<void> {
    return new Promise<void>(resolve => {
      this.getAuthState().pipe(
        take(1)
      ).subscribe(res => {
        const now = new Date();
        if (!AuthService.isHandling && res.isAuthenticated && res.expires.getTime() - now.getTime() < 60000) {
          const iframe = document.getElementById('silent-refresh-iframe');
          (iframe as any).src = this.createAuthorizeUrl(true);
          AuthService.isHandling = true;
        }
        resolve();
      });
    });
  }

  silentRefreshCallback(url: string) {
    const query = url.split('?')[1];
    if (query.includes('code')) {
      const params = new URLSearchParams(query);
      const authcode = params.get('code');
      this.onAuthCodeReceived(authcode, true);
    } else {
      this.onPostLogout();
      this.router.navigate(['/welcome']);
    }
  }
}
