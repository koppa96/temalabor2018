import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CurrentGameListPageComponent } from './pages/current-game-list/current-game-list.page.component';
import { PreviousGameListPageComponent } from './pages/previous-game-list/previous-game-list.page.component';
import { GameDetailsPageComponent } from './pages/game-details/game-details-page.component';
import { Connect4Component } from './components/games/connect4/connect4.component';
import { ChessComponent } from './components/games/chess/chess.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'current'
  },
  {
    path: 'current',
    component: CurrentGameListPageComponent
  },
  {
    path: 'previous',
    component: PreviousGameListPageComponent
  },
  {
    path: ':matchId',
    component: GameDetailsPageComponent,
    children: [
      {
        path: '0',
        component: Connect4Component
      },
      {
        path: '1',
        component: ChessComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GamesRoutingModule { }
