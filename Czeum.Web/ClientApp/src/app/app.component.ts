import { Component } from '@angular/core';
import { AuthService } from './authentication/services/authService';
import { Observable } from 'rxjs';
import { AuthState, State } from './reducers';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  authState: Observable<AuthState>;

  constructor(private authService: AuthService, private store: Store<State>) {
    this.authState = this.store.select(x => x.authState);
  }

  login() {
    this.authService.initiateAuthCodeFlow();
  }

  logout() {
    this.authService.initiateLogout();
  }
}
