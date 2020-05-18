import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SharedModule } from './shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { reducers } from './reducers';
import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule } from '@angular/common/http';
import { AuthenticationModule } from './authentication/authentication.module';
import { storageSyncMetaReducer } from 'ngrx-store-persist';
import { API_BASE_URL, MatchesClient } from './shared/clients';
import { AuthInterceptor } from './authentication/services/auth.interceptor';
import { environment } from '../environments/environment';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    SharedModule,
    HttpClientModule,
    StoreModule.forRoot(reducers, { metaReducers: [ storageSyncMetaReducer ] }),
    AuthenticationModule,
  ],
  providers: [
    HttpClient,
    { provide: API_BASE_URL, useValue: environment.apiBaseUrl },
    MatchesClient,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
