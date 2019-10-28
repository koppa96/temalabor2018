import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { FieldValidator } from '../../utility/field-validator';
import { ResetPasswordData } from '../../models/auth-models';

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

  constructor(
    formBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.resetPasswordForm = formBuilder.group({
      password: '',
      confirmPassword: '',
      token: ''
    });
    this.validator = new FieldValidator(this.resetPasswordForm);
  }

  ngOnInit() {
  }

  onSubmit(formData: ResetPasswordData) {
    this.resetPasswordForm.markAllAsTouched();
    if (this.resetPasswordForm.valid) {
      this.isLoading = true;
      this.authService.resetPassword(formData)
        .subscribe(
          () => this.resetSuccessful = true,
          () => this.resetFailed = true

      ).add(
        () => this.isLoading = false
      );
    }
  }

}
