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
import { FormsModule } from '@angular/forms';
import { GameDetailsPageComponent } from './pages/game-details/game-details-page.component';
import { ChessComponent } from './components/games/chess/chess.component';
import { Connect4Component } from './components/games/connect4/connect4.component';
import { GameChatComponent } from './components/game-chat/game-chat.component';

@NgModule({
  declarations: [
    CurrentGameListPageComponent,
    PreviousGameListPageComponent,
    CurrentGameListComponent,
    PreviousGameListComponent,
    GameListElementComponent,
    GameDetailsPageComponent,
    ChessComponent,
    Connect4Component,
    GameChatComponent
  ],
  imports: [
    CommonModule,
    GamesRoutingModule,
    SharedModule,
    FormsModule
  ],
  providers: [
    MatchService,
    GameIconMapperService
  ]
})
export class GamesModule {
}
