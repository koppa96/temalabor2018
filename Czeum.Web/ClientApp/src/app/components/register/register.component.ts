import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup} from '@angular/forms';
import {RegisterData} from '../../models/auth-models';
import { FieldValidator } from '../../utility/field-validator';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  isLoading = false;
  validator: FieldValidator;

  constructor(formBuilder: FormBuilder) {
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

  }


}
