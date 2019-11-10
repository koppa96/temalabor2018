import { Observable } from 'rxjs';

export class BackendValidatorContext {
  constructor(
    private self: any,
    private method: (self: any, value: any) => Observable<boolean>
  ) { }

  invoke(value: any): Observable<boolean> {
    return this.method(this.self, value);
  }
}
