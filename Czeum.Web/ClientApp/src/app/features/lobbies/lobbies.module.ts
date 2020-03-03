import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LobbyListPageComponent } from './pages/lobby-list/lobby-list.page.component';
import { LobbyDetailsPageComponent } from './pages/lobby-details/lobby-details.page.component';
import { LobbiesRoutingModule } from './lobbies-routing.module';

@NgModule({
  declarations: [
    LobbyListPageComponent,
    LobbyDetailsPageComponent],
  imports: [
    CommonModule,
    LobbiesRoutingModule
  ]
})
export class LobbiesModule { }
