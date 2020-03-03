import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FriendsPageComponent } from './pages/friends.page.component';
import { FriendsRoutingModule } from './friends-routing.module';

@NgModule({
  declarations: [FriendsPageComponent],
  imports: [
    CommonModule,
    FriendsRoutingModule
  ]
})
export class FriendsModule { }
