import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WelcomePageComponent } from './pages/welcome.page.component';
import { WelcomeRoutingModule } from './welcome-routing.module';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  declarations: [
    WelcomePageComponent
  ],
  imports: [
    CommonModule,
    WelcomeRoutingModule,
    SharedModule
  ]
})
export class WelcomeModule { }
