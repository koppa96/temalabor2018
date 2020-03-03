import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CurrentGameListPageComponent } from './pages/current-game-list/current-game-list.page.component';
import { PreviousGameListPageComponent } from './pages/previous-game-list/previous-game-list.page.component';

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
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GamesRoutingModule { }
