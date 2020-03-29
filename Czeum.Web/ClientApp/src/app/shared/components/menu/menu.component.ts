import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../authentication/services/authService';
import { AuthState } from '../../../reducers';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})
export class MenuComponent implements OnInit {
  authState$: Observable<AuthState>;
  submenuOpen = false;

  constructor(private authService: AuthService) {
    this.authState$ = this.authService.getAuthState();
  }

  ngOnInit() {
  }

  closeSubmenu() {
    this.submenuOpen = false;
  }

  toggleSubmenu() {
    this.submenuOpen = !this.submenuOpen;
    console.log(this.submenuOpen);
  }

  login() {
    this.authService.initiateAuthCodeFlow();
  }

  logout() {
    this.authService.initiateLogout();
  }
}
