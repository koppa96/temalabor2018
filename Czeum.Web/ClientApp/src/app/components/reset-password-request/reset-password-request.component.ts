import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ResetPasswordRequestData } from '../../models/auth-models';
import { FieldValidator } from '../../utility/field-validator';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password-request.component.html',
  styleUrls: ['./reset-password-request.component.css']
})
export class ResetPasswordRequestComponent implements OnInit {
  resetPasswordRequestForm: FormGroup;
  validator: FieldValidator;
  isLoading = false;
  requestSuccessful = false;
  requestUnsuccessful = false;

  constructor(
    formBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.resetPasswordRequestForm = formBuilder.group({
      username: '',
      email: ''
    });

    this.validator = new FieldValidator(this.resetPasswordRequestForm);
  }

  ngOnInit() {
  }

  onSubmit(requestData: ResetPasswordRequestData) {
    this.resetPasswordRequestForm.markAllAsTouched();
    if (this.resetPasswordRequestForm.valid) {
      this.isLoading = true;
      this.requestSuccessful = false;
      this.requestUnsuccessful = false;
      this.authService.requestResetPassword(requestData)
        .subscribe(
          () => this.requestSuccessful = true,
          () => this.requestUnsuccessful = true,
        ).add(() => this.isLoading = false);
    }
  }

}
