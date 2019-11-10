import { Component, OnDestroy, OnInit } from '@angular/core';
import {AuthService} from '../../services/auth.service';
import { AuthEventListener } from '../../interfaces/AuthEventListener';
import { UserInfo } from '../../models/auth-models';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit, AuthEventListener, OnDestroy {
  isLoading = false;
  userInfo: UserInfo;

  constructor(private authService: AuthService) {
    this.authService.addAuthEventListener(this);
    this.isLoading = true;
    this.authService.refresh()
      .then(() => this.isLoading = false)
      .catch(() => this.isLoading = false);
  }

  ngOnInit() {
  }

  onLoggedIn(userInfo: UserInfo) {
    this.userInfo = userInfo;
  }

  onLoggedOut() {
    this.userInfo = null;
  }

  logout() {
    this.authService.logout();
    location.reload();
  }

  ngOnDestroy(): void {
    this.authService.removeAuthEventListener(this);
  }
}
