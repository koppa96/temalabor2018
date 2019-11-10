import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { FieldValidator } from '../../utility/field-validator';

@Component({
  selector: 'app-resend-confirmation-email',
  templateUrl: './resend-confirmation-email.component.html',
  styleUrls: ['./resend-confirmation-email.component.css']
})
export class ResendConfirmationEmailComponent implements OnInit {
  resendEmailForm: FormGroup;
  validator: FieldValidator;
  isLoading = false;
  resendSuccessful = false;
  resendFailed = false;

  constructor(
    formBuilder: FormBuilder,
    private authService: AuthService
  ) {
    this.resendEmailForm = formBuilder.group({
      email: ''
    });

    this.validator = new FieldValidator(this.resendEmailForm);
  }

  ngOnInit() {
  }

  onSubmit(formData: {email: string}) {
    this.resendEmailForm.markAllAsTouched();
    if (this.resendEmailForm.valid) {
      this.authService.resendConfirmationEmail(formData.email)
        .subscribe(
          () => {}
        );
    }

  }
}
