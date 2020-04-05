import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from './authentication/services/auth.service';
import { Observable } from 'rxjs';
import { AuthState, State } from './reducers';
import { Store } from '@ngrx/store';
import { take } from 'rxjs/operators';
import { HubService } from './shared/services/hub.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  interval: any;
  authState: Observable<AuthState>;
  showInitialLoading: boolean;

  constructor(private authService: AuthService, private hubService: HubService) {
    this.authState = this.authService.getAuthState();
  }

  async ngOnInit() {
    this.showInitialLoading = true;
    await this.authService.silentRefreshIfRequired();
    if (AuthService.isHandling) {
      this.authService.addAuthenticationCallback(async () => {
        this.showInitialLoading = false;
        await this.hubService.connect();
      });
    } else {
      this.showInitialLoading = false;
      await this.hubService.connect();
    }

    const self = this;
    this.interval = setInterval(async () => {
      await self.authService.silentRefreshIfRequired();
    }, 60000);
  }

  async ngOnDestroy() {
    clearInterval(this.interval);
    await this.hubService.disconnect();
  }
}
