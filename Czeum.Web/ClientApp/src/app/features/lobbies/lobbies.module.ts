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
import { LobbyService } from './services/lobby.service';
import { LobbyCreateComponent } from './components/lobby-create/lobby-create.component';
import { FormsModule } from '@angular/forms';
import { LobbyListItemComponent } from './components/lobby-list-item/lobby-list-item.component';

@NgModule({
  declarations: [
    LobbyListPageComponent,
    LobbyDetailsPageComponent,
    MyLobbyComponent,
    GameSettingsComponent,
    LobbyChatComponent,
    LobbyListComponent,
    LobbyCreateComponent,
    LobbyListItemComponent
  ],
  imports: [
    CommonModule,
    LobbiesRoutingModule,
    SharedModule,
    FormsModule
  ],
  providers: [
    LobbyService
  ]
})
export class LobbiesModule { }
