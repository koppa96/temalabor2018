import { Component, Input, OnInit } from '@angular/core';
import { MatchStatus } from '../../../../shared/clients';
import { faChessPawn, faCircle } from '@fortawesome/free-solid-svg-icons';
import { GameIconMapperService } from '../../services/game-icon-mapper.service';
import { IconDefinition } from '@fortawesome/fontawesome-common-types';

@Component({
  selector: 'app-game-list-element',
  templateUrl: './game-list-element.component.html',
  styleUrls: ['./game-list-element.component.scss']
})
export class GameListElementComponent implements OnInit {
  chessIcon = faChessPawn;
  connect4Icon = faCircle;

  @Input() data: MatchStatus;
  @Input() currentUserName = 'alma';

  constructor(private gameIconMapperService: GameIconMapperService) { }

  ngOnInit() {
  }

  getEnemies(): string {
    const enemies = this.data.players.filter(x => x.username !== this.currentUserName);
    let enemyNameString = enemies[enemies.length - 1].username;
    for (let i = enemies.length - 2; i >= 0; i++) {
      enemyNameString = enemies[i].username + ', ' + enemyNameString;
    }
    return enemyNameString;
  }

  getIcon(gameIdentifier: number): IconDefinition {
    return this.gameIconMapperService.mapIcon(gameIdentifier);
  }

}
