import { Directive, Input } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator } from '@angular/forms';

@Directive({
  selector: '[appMatchValue]',
  providers: [
    { provide: NG_VALIDATORS, useExisting: MatchValueDirective, multi: true }
  ]
})
export class MatchValueDirective implements Validator {
  @Input() comparedField: string;

  constructor() { }

  validate(control: AbstractControl): ValidationErrors | null {
    const value = control.value;
    const referenceValue = control.root.get(this.comparedField).value;

    if (value !== referenceValue) {
      return { matchValue: true };
    }

    return null;
  }
}
