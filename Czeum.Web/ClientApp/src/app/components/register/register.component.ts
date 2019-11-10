import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {RegisterData} from '../../models/auth-models';
import { FieldValidator } from '../../utility/field-validator';
import { BackendValidatorContext } from '../../models/backend-validator-context';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  isLoading = false;
  validator: FieldValidator;
  registerSuccessful = false;
  registerFailed = false;

  usernameValidationContext = new BackendValidatorContext(
    this.authService,
    (self, value) => self.usernameAvailable(value)
  );

  emailValidationContext = new BackendValidatorContext(
    this.authService,
    (self, value) => self.emailAvailable(value)
  );

  constructor(
    formBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.registerForm = formBuilder.group({
      username: '',
      email: '',
      password: '',
      confirmPassword: ''
    });

    this.validator = new FieldValidator(this.registerForm);
  }

  ngOnInit() {
  }

  onSubmit(formData: RegisterData) {
    this.registerForm.markAllAsTouched();
    if (this.registerForm.valid) {
      this.isLoading = true;
      this.registerFailed = false;
      this.registerSuccessful = false;
      this.authService.register(formData)
        .then(
          () => {
            this.registerSuccessful = true;
            this.registerForm.reset();
            this.isLoading = false;
          }
        ).catch(
          () => {
            this.registerFailed = true;
            this.isLoading = false;
          }
        )
    }
  }

}
