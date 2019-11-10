import { FormGroup } from '@angular/forms';

export class FieldValidator {
  constructor(private form: FormGroup) { }

  fieldHasError(fieldName: string, error?: string): boolean {
    return error ?
      this.form.controls[fieldName].touched && this.form.controls[fieldName].hasError(error) :
      this.form.controls[fieldName].touched && this.form.controls[fieldName].invalid;
  }

  notEmptyAndHasError(fieldName: string, error: string): boolean {
    return !this.form.controls[fieldName].hasError('required') &&
      this.form.controls[fieldName].hasError(error);
  }
}
