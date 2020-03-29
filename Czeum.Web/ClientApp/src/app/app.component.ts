import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from './authentication/services/authService';
import { Observable } from 'rxjs';
import { AuthState, State } from './reducers';
import { Store } from '@ngrx/store';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  interval: any;
  authState: Observable<AuthState>;

  constructor(private authService: AuthService) {
    this.authState = this.authService.getAuthState();
  }

  ngOnInit() {
    this.authService.silentRefreshIfRequired();

    /*const self = this;
    this.interval = setInterval(() => {
      this.authService.silentRefreshIfRequired();
    }, 60000);*/
  }

  ngOnDestroy() {
    clearInterval(this.interval);
  }

  login() {
    this.authService.initiateAuthCodeFlow();
  }

  logout() {
    this.authService.initiateLogout();
  }
}
