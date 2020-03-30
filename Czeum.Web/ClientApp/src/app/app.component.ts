import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from './authentication/services/auth.service';
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
  showInitialLoading: boolean;

  constructor(private authService: AuthService) {
    this.authState = this.authService.getAuthState();
  }

  ngOnInit() {
    this.showInitialLoading = true;
    this.authService.silentRefreshIfRequired().then(() => {
      if (AuthService.isHandling) {
        this.authService.addAuthenticationCallback(() => {
          this.showInitialLoading = false;
        });
      } else {
        this.showInitialLoading = false;
      }
    });

    const self = this;
    this.interval = setInterval(() => {
      self.authService.silentRefreshIfRequired();
    }, 60000);
  }

  ngOnDestroy() {
    clearInterval(this.interval);
  }
}
