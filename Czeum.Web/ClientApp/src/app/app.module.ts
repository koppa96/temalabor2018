import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './components/app.component';
import { MenuComponent } from './components/menu/menu.component';
import { HomeComponent } from './components/home/home.component';
import { AuthService } from './services/auth.service';
import { AuthInterceptor } from './interceptors/auth-interceptor';
import { LoginComponent } from './components/login/login.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { AuthGuard } from './auth-guard/auth-guard';
import { RegisterComponent } from './components/register/register.component';
import { PasswordValidatorDirective } from './directives/password-validator.directive';
import { MatchValueDirective } from './directives/match-value.directive';
import { ReverseMatchValueDirective } from './directives/reverse-match-value.directive';
import { BackendValidatorDirectiveDirective } from './directives/backend-validator-directive.directive';
import { DetailsComponent } from './components/details/details.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { ResetPasswordRequestComponent } from './components/reset-password-request/reset-password-request.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    HomeComponent,
    LoginComponent,
    ChangePasswordComponent,
    RegisterComponent,
    PasswordValidatorDirective,
    MatchValueDirective,
    ReverseMatchValueDirective,
    BackendValidatorDirectiveDirective,
    DetailsComponent,
    ConfirmEmailComponent,
    ResetPasswordRequestComponent,
    ResetPasswordComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'home', pathMatch: 'full' },
      { path: 'home', component: HomeComponent },
      { path: 'login', component: LoginComponent },
      { path: 'register', component: RegisterComponent },
      { path: 'change-password', component: ChangePasswordComponent, canActivate: [ AuthGuard ] },
      { path: 'details', component: DetailsComponent },
      { path: 'reset-password-request', component: ResetPasswordRequestComponent },
      { path: 'reset-password', component: ResetPasswordComponent }
    ]),
    ReactiveFormsModule
  ],
  providers: [
    HttpClient,
    AuthService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
    FormBuilder,
    AuthGuard
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule {
}
