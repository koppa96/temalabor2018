import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../../authentication/services/authService';

@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.page.component.html',
  styleUrls: ['./welcome.page.component.scss']
})
export class WelcomePageComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
  }

  redirectToRegister() {
    const params = new URLSearchParams();
    params.append('returnUrl', window.location.href);

    window.location.href = 'https://localhost:5001/Account/Register?' + params.toString();
  }

  redirectToPasswordReset() {
    const params = new URLSearchParams();
    params.append('returnUrl', window.location.href);

    window.location.href = 'https://localhost:5001/Account/GetPasswordReset?' + params.toString();
  }

  redirectToResendConfirmationEmail() {
    const params = new URLSearchParams();
    params.append('returnUrl', window.location.href);
    params.append('resend', 'true');

    window.location.href = 'https://localhost:5001/Account/ConfirmEmail?' + params.toString();
  }

  redirectToLogin() {
    this.authService.initiateAuthCodeFlow();
  }

}
