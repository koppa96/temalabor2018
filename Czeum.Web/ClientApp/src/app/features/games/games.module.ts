import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CurrentGameListPageComponent } from './pages/current-game-list/current-game-list.page.component';
import { PreviousGameListPageComponent } from './pages/previous-game-list/previous-game-list.page.component';
import { GamesRoutingModule } from './games-routing.module';
import { CurrentGameListComponent } from './components/current-game-list/current-game-list.component';
import { PreviousGameListComponent } from './components/previous-game-list/previous-game-list.component';
import { SharedModule } from '../../shared/shared.module';
import { MatchService } from './services/match.service';
import { GameListElementComponent } from './components/game-list-element/game-list-element.component';
import { GameIconMapperService } from './services/game-icon-mapper.service';

@NgModule({
  declarations: [
    CurrentGameListPageComponent,
    PreviousGameListPageComponent,
    CurrentGameListComponent,
    PreviousGameListComponent,
    GameListElementComponent
  ],
  imports: [
    CommonModule,
    GamesRoutingModule,
    SharedModule
  ],
  providers: [
    MatchService,
    GameIconMapperService
  ]
})
export class GamesModule { }
