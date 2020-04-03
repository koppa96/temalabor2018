import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CurrentGameListPageComponent } from './pages/current-game-list/current-game-list.page.component';
import { PreviousGameListPageComponent } from './pages/previous-game-list/previous-game-list.page.component';
import { GamesRoutingModule } from './games-routing.module';
import { CurrentGameListComponent } from './components/current-game-list/current-game-list.component';
import { PreviousGameListComponent } from './components/previous-game-list/previous-game-list.component';
import { SharedModule } from '../../shared/shared.module';



@NgModule({
  declarations: [
    CurrentGameListPageComponent,
    PreviousGameListPageComponent,
    CurrentGameListComponent,
    PreviousGameListComponent
  ],
  imports: [
    CommonModule,
    GamesRoutingModule,
    SharedModule
  ]
})
export class GamesModule { }
