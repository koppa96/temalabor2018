import { Directive, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator } from '@angular/forms';

@Directive({
  selector: '[appReverseMatchValue]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: ReverseMatchValueDirective, multi: true }
  ]
})
export class ReverseMatchValueDirective implements Validator {
  @Input() comparedFieldNames: string[];

  constructor() { }

  validate(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    for (const fieldName of this.comparedFieldNames) {
      const comparedControl = control.root.get(fieldName);
      if (comparedControl.value !== value) {
        comparedControl.setErrors({ matchValue: true });
      } else if (comparedControl.errors && comparedControl.errors.matchValue) {
        delete comparedControl.errors.matchValue;
      }
    }

    return null;
  }
}
