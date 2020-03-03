import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LobbyListPageComponent } from './pages/lobby-list/lobby-list.page.component';
import { LobbyDetailsPageComponent } from './pages/lobby-details/lobby-details.page.component';

const routes: Routes = [
  {
    path: 'list',
    component: LobbyListPageComponent
  },
  {
    path: 'mine',
    component: LobbyDetailsPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LobbiesRoutingModule { }
