import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { LobbyService } from '../../services/lobby.service';
import { Observable } from 'rxjs';
import { GameTypeDto } from '../../../../shared/clients';
import { LobbyAccessDropdownItem, lobbyAccessDropdownItems, LobbyCreateDetails } from '../../models/lobby-create.models';

@Component({
  selector: 'app-lobby-create',
  templateUrl: './lobby-create.component.html',
  styleUrls: ['./lobby-create.component.scss']
})
export class LobbyCreateComponent implements OnInit {
  @Output() lobbyCreateInitiated = new EventEmitter<LobbyCreateDetails>();

  gameTypes: GameTypeDto[];
  selectedGameType: GameTypeDto;

  lobbyAccesses = lobbyAccessDropdownItems;
  selectedLobbyAccess: LobbyAccessDropdownItem;

  lobbyName: string;

  constructor(private lobbyService: LobbyService) { }

  ngOnInit() {
    this.selectedLobbyAccess = this.lobbyAccesses[0];
    this.lobbyService.getAvailableGameTypes().subscribe(res => {
      this.gameTypes = res;
      this.selectedGameType = this.gameTypes[0];
    });
  }

  onLobbyCreate() {
    this.lobbyCreateInitiated.emit({
      gameType: this.selectedGameType,
      lobbyAccess: this.selectedLobbyAccess,
      name: this.lobbyName
    });
  }

}
