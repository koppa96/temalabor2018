import { Directive } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator } from '@angular/forms';

@Directive({
  selector: '[appPasswordValidator]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: PasswordValidatorDirective, multi: true }
  ]
})
export class PasswordValidatorDirective implements Validator {

  constructor() { }

  validate(control: AbstractControl): ValidationErrors | null {
    if (!/[A-Z]/.test(control.value) || !/[a-z]/.test(control.value) || !/[0-9]/.test(control.value)) {
      return { password: true };
    }
    return null;
  }

}
