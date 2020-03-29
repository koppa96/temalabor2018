import { Inject, Injectable } from '@angular/core';
import { ClientConfig, ServerConfig } from '../auth-config-models';
import { Store } from '@ngrx/store';
import { AuthState, State } from '../../reducers';
import { take } from 'rxjs/operators';
import { updateAuthState, updatePkceString } from '../../reducers/authentication/auth-actions';
import { CLIENT_CONFIG, SERVER_CONFIG } from '../dependecy-injection/config-injections';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { create } from 'pkce';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(
    @Inject(CLIENT_CONFIG) private clientConfig: ClientConfig,
    @Inject(SERVER_CONFIG) private serverConfig: ServerConfig,
    private store: Store<State>,
    private http: HttpClient) {
  }
  
  initiateAuthCodeFlow() {
    const codePair = create();
    this.store.dispatch(updatePkceString({ updatedString: codePair.codeVerifier }));

    const params = new URLSearchParams();
    params.append('client_id', this.clientConfig.clientId);
    params.append('scope', this.clientConfig.scope);
    params.append('response_type', this.clientConfig.responseType);
    params.append('redirect_uri', this.clientConfig.postLoginRedirectUri);
    params.append('code_challenge', codePair.codeChallenge);
    params.append('code_challenge_method', 'S256');
    window.location.href = `${this.serverConfig.authorizeUrl}?${params.toString()}`;
  }

  onAuthCodeReceived(authCode: string): Promise<void> {
    return new Promise<void>((resolve, reject) => {
      this.store.select(x => x.pkceString).pipe(
        take(1)
      ).subscribe(res => {
        const params = new URLSearchParams();
        params.append('client_id', this.clientConfig.clientId);
        params.append('scope', this.clientConfig.scope);
        params.append('grant_type', 'authorization_code');
        params.append('code', authCode);
        params.append('redirect_uri', this.clientConfig.postLoginRedirectUri);
        params.append('code_verifier', res);

        this.http.post(this.serverConfig.tokenUrl, params.toString(), {
          headers: new HttpHeaders({
            'Content-Type': 'application/x-www-form-urlencoded'
          })
        }).subscribe((tokenResponse: { id_token: string; access_token: string; }) => {
          this.onPostLogin(tokenResponse.id_token, tokenResponse.access_token);
          resolve();
        },
          () => reject()
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
        console.log(res.idToken);
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
}
