import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LobbyListPageComponent } from './pages/lobby-list/lobby-list.page.component';
import { LobbyDetailsPageComponent } from './pages/lobby-details/lobby-details.page.component';
import { LobbiesRoutingModule } from './lobbies-routing.module';
import { MyLobbyComponent } from './components/my-lobby/my-lobby.component';
import { GameSettingsComponent } from './components/game-settings/game-settings.component';
import { SharedModule } from '../../shared/shared.module';
import { LobbyChatComponent } from './components/lobby-chat/lobby-chat.component';
import { LobbyListComponent } from './components/lobby-list/lobby-list.component';

@NgModule({
  declarations: [
    LobbyListPageComponent,
    LobbyDetailsPageComponent,
    MyLobbyComponent,
    GameSettingsComponent,
    LobbyChatComponent,
    LobbyListComponent],
  imports: [
    CommonModule,
    LobbiesRoutingModule,
    SharedModule
  ]
})
export class LobbiesModule { }
