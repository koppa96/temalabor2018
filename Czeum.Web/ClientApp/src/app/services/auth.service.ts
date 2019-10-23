import {Inject, Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {LoginData, RegisterData, TokenResponse, UserInfo} from '../models/auth-models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient, @Inject('API_URL') private apiUrl) { }

  private postTokenRequest(formContent: URLSearchParams): Promise<any> {
    const options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/x-www-form-urlencoded'
      })
    };

    return new Promise((resolve, reject) => {
      this.http.post<TokenResponse>(this.apiUrl + '/connect/token', formContent.toString(), options)
        .subscribe(
          resp => {
            localStorage.setItem('access_token', resp.access_token);
            localStorage.setItem('refresh_token', resp.refresh_token);
            resolve();
          },
          () => {
            reject();
          }
        );
    });
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('access_token');
  }

  getUserInfo(): Observable<UserInfo> {
    return this.http.get<UserInfo>(this.apiUrl + '/api/accounts/me');
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
    form.append('client_id', 'ChitchatUWPClient');
    form.append('client_secret', 'UWPClientSecret');
    form.append('grant_type', 'refresh_token');
    form.append('refresh_token', refreshToken);
    form.append('scope', 'offline_access chitchat_api');

    return this.postTokenRequest(form);
  }

  register(registerData: RegisterData): Promise<any> {
    return new Promise((resolve, reject) => {
      this.http.post(this.apiUrl + '/api/accounts', registerData)
        .subscribe(
          () => resolve(),
          () => reject());
    });
  }

  logout() {
    localStorage.removeItem('access_token');
    localStorage.removeItem('refresh_token');
  }

  usernameAvailable(username: string): Observable<boolean> {
    console.log(this);

    const query = new URLSearchParams();
    query.append('username', username);

    return this.http.get<boolean>(this.apiUrl + '/api/accounts/is-username-free?' + query.toString());
  }

  emailAvailable(email: string): Observable<boolean> {
    const query = new URLSearchParams();
    query.append('email', email);

    return this.http.get<boolean>(this.apiUrl + '/api/accounts/is-email-free?' + query.toString());
  }
}
