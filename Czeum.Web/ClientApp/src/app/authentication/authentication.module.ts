import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SigninComponent } from './components/signin/signin.component';
import { SharedModule } from '../shared/shared.module';
import { SignoutComponent } from './components/signout/signout.component';

@NgModule({
  declarations: [
    SigninComponent,
    SignoutComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
  ],
  exports: [
    SigninComponent,
    SignoutComponent
  ]
})
export class AuthenticationModule { }
