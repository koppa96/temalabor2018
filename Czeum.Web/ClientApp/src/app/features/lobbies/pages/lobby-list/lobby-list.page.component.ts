import { Component, OnInit } from '@angular/core';
import { LobbyService } from '../../services/lobby.service';
import { GameTypeDto, LobbyDataWrapper } from '../../../../shared/clients';
import { Router } from '@angular/router';

@Component({
  templateUrl: './lobby-list.page.component.html',
  styleUrls: ['./lobby-list.page.component.scss']
})
export class LobbyListPageComponent implements OnInit {
  gameTypes: GameTypeDto[] = [];
  lobbies: LobbyDataWrapper[] = [];

  constructor(private lobbyService: LobbyService, private router: Router) { }

  ngOnInit() {
    this.lobbyService.getAvailableGameTypes().subscribe(res => {
      this.gameTypes = res;
    });

    this.lobbyService.getLobbies().subscribe(res => {
      this.lobbies = res;
    });
  }

  joinLobby(lobbyId: string) {
    this.lobbyService.joinLobby(lobbyId).subscribe(() => {
      this.router.navigate(['/lobbies/mine']);
    });
  }

}
