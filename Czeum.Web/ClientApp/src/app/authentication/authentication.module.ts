import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SigninComponent } from './components/signin/signin.component';
import { SignoutComponent } from './components/signout/signout.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  declarations: [
    SigninComponent,
    SignoutComponent
  ],
  imports: [
    CommonModule,
    MatProgressSpinnerModule
  ],
  exports: [
    SigninComponent,
    SignoutComponent
  ]
})
export class AuthenticationModule { }
