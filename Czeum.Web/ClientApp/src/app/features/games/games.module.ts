import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CurrentGameListPageComponent } from './pages/current-game-list/current-game-list.page.component';
import { PreviousGameListPageComponent } from './pages/previous-game-list/previous-game-list.page.component';
import { GamesRoutingModule } from './games-routing.module';



@NgModule({
  declarations: [
    CurrentGameListPageComponent,
    PreviousGameListPageComponent
  ],
  imports: [
    CommonModule,
    GamesRoutingModule
  ]
})
export class GamesModule { }
