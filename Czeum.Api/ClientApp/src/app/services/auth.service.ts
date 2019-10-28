import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {
  ChangePasswordData, ConfirmEmailData,
  LoginData,
  RegisterData,
  ResetPasswordData,
  ResetPasswordRequestData,
  TokenResponse,
  UserInfo
} from '../models/auth-models';
import { AuthEventListener } from '../interfaces/AuthEventListener';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private authEventListeners: AuthEventListener[] = [];

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private apiUrl: string
  ) { }

  private postTokenRequest(formContent: URLSearchParams): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded'
      })
    };

    return new Promise<any>((resolve, reject) => {
      this.http.post<TokenResponse>(this.apiUrl + 'connect/token', formContent.toString(), options)
        .subscribe(
          resp => {
            localStorage.setItem('access_token', resp.access_token);
            localStorage.setItem('refresh_token', resp.refresh_token);
            this.getUserInfo().subscribe(
              userInfo => {
                for (const listener of this.authEventListeners) {
                  if (listener.onLoggedIn) {
                    listener.onLoggedIn(userInfo);
                  }
                }
                resolve();
              }
            );
          },
          () => {
            this.logout();
            reject();
          }
        );
    });
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('access_token');
  }

  getUserInfo(): Observable<UserInfo> {
    return this.http.get<UserInfo>(this.apiUrl + 'api/accounts/me');
  }

  getToken(): string | null {
    return localStorage.getItem('access_token');
  }

  login(loginData: LoginData): Promise<any> {
    const form = new URLSearchParams();
    form.append('client_id', 'CzeumAngularClient');
    form.append('client_secret', 'CzeumAngularClientSecret');
    form.append('grant_type', 'password');
    form.append('username', loginData.username);
    form.append('password', loginData.password);
    form.append('scope', 'offline_access czeum_api');

    return this.postTokenRequest(form);
  }

  refresh(): Promise<any> {
    const refreshToken = localStorage.getItem('refresh_token');
    if (!refreshToken) {
      return new Promise((resolve, reject) => reject());
    }

    const form = new URLSearchParams();
    form.append('client_id', 'CzeumAngularClient');
    form.append('client_secret', 'CzeumAngularClientSecret');
    form.append('grant_type', 'refresh_token');
    form.append('refresh_token', refreshToken);
    form.append('scope', 'offline_access chitchat_api');

    return this.postTokenRequest(form);
  }

  register(registerData: RegisterData): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.post(this.apiUrl + 'api/accounts/register', registerData)
        .subscribe(
          () => resolve(),
          () => reject());
    });
  }

  logout() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');

    for (const listener of this.authEventListeners) {
      if (listener.onLoggedOut) {
        listener.onLoggedOut();
      }
    }
  }

  changePassword(changePasswordData: ChangePasswordData): Observable<any> {
    return this.http.post(this.apiUrl + 'api/accounts/change-password', changePasswordData);
  }

  usernameAvailable(username: string): Observable<boolean> {
    console.log(this);

    const query = new URLSearchParams();
    query.append('username', username);

    return this.http.get<boolean>(this.apiUrl + 'api/accounts/username-available?' + query.toString());
  }

  emailAvailable(email: string): Observable<boolean> {
    const query = new URLSearchParams();
    query.append('email', email);

    return this.http.get<boolean>(this.apiUrl + 'api/accounts/email-available?' + query.toString());
  }

  addAuthEventListener(listener: AuthEventListener) {
    this.authEventListeners.push(listener);
  }

  removeAuthEventListener(listener: AuthEventListener) {
    this.authEventListeners.splice(this.authEventListeners.findIndex(x => x === listener), 1);
  }

  requestResetPassword(requestData: ResetPasswordRequestData) {
    const query = new URLSearchParams();
    query.append('username', requestData.username);
    query.append('email', requestData.email);

    return this.http.get(this.apiUrl + 'api/accounts/reset-password?' + query.toString());
  }

  resetPassword(resetPasswordData: ResetPasswordData) {
    return this.http.post(this.apiUrl + 'api/accounts/reset-password', resetPasswordData);
  }

  resendConfirmationEmail(email: string) {
    return this.http.get(this.apiUrl + 'api/accounts/resend-confirm-email');
  }

  confirmEmail(confirmEmailData: ConfirmEmailData) {
    const query = new URLSearchParams();
    query.append('username', confirmEmailData.username);
    query.append('token', confirmEmailData.token);

    return this.http.post(this.apiUrl + 'api/accounts/confirm-email?' + query.toString(), null);
  }
}
