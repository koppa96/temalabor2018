import { Directive, Input } from '@angular/core';
import { AbstractControl, NG_ASYNC_VALIDATORS, ValidationErrors } from '@angular/forms';
import { BackendValidatorContext } from '../models/backend-validator-context';
import { Observable, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';

@Directive({
  selector: '[appBackendValidator]',
  providers: [
    { provide: NG_ASYNC_VALIDATORS, useExisting: BackendValidatorDirectiveDirective, multi: true }
  ]
})
export class BackendValidatorDirectiveDirective {
  @Input() context: BackendValidatorContext;

  constructor() { }

  validate(control: AbstractControl): Promise<ValidationErrors | null> | Observable<ValidationErrors | null> {
    const value = control.value;
    return timer(500).pipe(
      switchMap(() => {
        if (!value) {
          return null;
        }

        return this.context.invoke(value).pipe(
          map(result => result ? null : { backend: true })
        );
      })
    );
  }
}
