import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {ActivatedRoute, Router} from '@angular/router';
import {LoginData} from '../../models/auth-models';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private loginForm: FormGroup;
  private redirectUrl: string | null;
  private loginUnsuccessful = false;
  private isLoading = false;

  constructor(
    formBuilder: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    route.queryParams.subscribe(
      params => this.redirectUrl = params.redirectUrl
    );

    this.loginForm = formBuilder.group({
      username: '',
      password: ''
    });

    this.isLoading = true;
    this.authService.refresh()
      .then(
        () => this.onSuccessfulLogin()
      ).finally(
        () => this.isLoading = false
      );
  }

  private onSuccessfulLogin() {
    if (this.redirectUrl) {
      return this.router.navigate([this.redirectUrl]);
    } else {
      return this.router.navigate(['']);
    }
  }

  ngOnInit() {
  }

  onSubmit(formData: LoginData) {
    this.loginForm.markAllAsTouched();
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.authService.login(formData)
        .then(
          () => this.onSuccessfulLogin()
        ).catch(
          () => {
            this.loginUnsuccessful = true;
            this.loginForm.reset();
          }
        ).finally(
          () => this.isLoading = false
        );
    }
  }

  fieldHasError(fieldName: string, error?: string): boolean {
    return error ?
      this.loginForm.controls[fieldName].touched && this.loginForm.controls[fieldName].hasError(error) :
      this.loginForm.controls[fieldName].touched && this.loginForm.controls[fieldName].invalid;
  }
}
