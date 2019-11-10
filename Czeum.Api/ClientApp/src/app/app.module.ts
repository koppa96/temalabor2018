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
import { ResendConfirmationEmailComponent } from './components/resend-confirmation-email/resend-confirmation-email.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FriendsComponent } from './components/friends/friends.component';
import { FriendListComponent } from './components/friend-list/friend-list.component';
import { SentRequestListComponent } from './components/sent-request-list/sent-request-list.component';
import { ReceivedRequestListComponent } from './components/received-request-list/received-request-list.component';
import { NewRequestComponent } from './components/new-request/new-request.component';

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
    ResendConfirmationEmailComponent,
    FriendsComponent,
    FriendListComponent,
    SentRequestListComponent,
    ReceivedRequestListComponent,
    NewRequestComponent
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
      { path: 'reset-password', component: ResetPasswordComponent },
      { path: 'confirm-email', component: ConfirmEmailComponent },
      {
        path: 'friends', 
        component: FriendsComponent,
        children: [
          { path: '', redirectTo: 'friend-list', pathMatch: 'full' },
          { path: 'friend-list', component: FriendListComponent },
          { path: 'sent-requests', component: SentRequestListComponent },
          { path: 'received-requests', component: ReceivedRequestListComponent },
          { path: 'new-request', component: NewRequestComponent }
        ]
      }
    ]),
    ReactiveFormsModule,
    FontAwesomeModule
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
