import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { FieldValidator } from '../../utility/field-validator';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  validator: FieldValidator;
  isLoading = false;
  resetSuccessful = false;
  resetFailed = false;
  username: string;
  token: string;

  constructor(
    route: ActivatedRoute,
    formBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.resetPasswordForm = formBuilder.group({
      password: '',
      confirmPassword: ''
    });
    this.validator = new FieldValidator(this.resetPasswordForm);

    route.queryParams.subscribe(params => {
      this.username = params.username;
      this.token = params.token;
    });
  }

  ngOnInit() {
  }

  onSubmit(formData: { password: string; confirmPassword: string }) {
    this.resetPasswordForm.markAllAsTouched();
    if (this.resetPasswordForm.valid) {
      this.isLoading = true;
      this.resetSuccessful = false;
      this.resetFailed = false;
      this.authService.resetPassword({
        username: this.username,
        token: this.token,
        password: formData.password
      })
        .subscribe(
          () => this.resetSuccessful = true,
          () => this.resetFailed = true

      ).add(
        () => this.isLoading = false
      );
    }
  }

}
