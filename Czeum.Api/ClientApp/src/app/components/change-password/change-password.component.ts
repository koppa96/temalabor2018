import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ChangePasswordData } from '../../models/auth-models';
import { FieldValidator } from '../../utility/field-validator';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  changePasswordForm: FormGroup;
  isLoading = false;
  validator: FieldValidator;
  changePasswordSuccessful = false;
  changePasswordFailed = false;

  constructor(
    formBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.changePasswordForm = formBuilder.group({
      oldPassword: '',
      password: '',
      confirmPassword: ''
    });

    this.validator = new FieldValidator(this.changePasswordForm);
  }

  ngOnInit() {
  }

  onSubmit(formData: ChangePasswordData) {
    this.changePasswordForm.markAllAsTouched();
    if (this.changePasswordForm.valid) {
      this.isLoading = true;
      this.authService.changePassword(formData).subscribe(
        () => {
          this.changePasswordSuccessful = true;
        },
        () => {
          this.changePasswordFailed = true;
        }
      ).add(() => {
        this.isLoading = false;
      });
    }
  }

  onPasswordChanged() {
    this.changePasswordFailed = false;
  }
}
