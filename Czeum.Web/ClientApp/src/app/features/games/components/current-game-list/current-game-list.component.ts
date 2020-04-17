import { Component, Input, OnInit } from '@angular/core';
import { GameState, MatchStatus } from '../../../../shared/clients';
import { RollList } from '../../../../shared/models/roll-list';

@Component({
  selector: 'app-current-game-list',
  templateUrl: './current-game-list.component.html',
  styleUrls: ['./current-game-list.component.scss']
})
export class CurrentGameListComponent implements OnInit {
  @Input() matches: RollList<MatchStatus> = new RollList<MatchStatus>([
    {
      id: '1',
      createDate: new Date(),
      currentBoard: {
        gameIdentifier: 0,
        content: undefined
      },
      currentPlayerIndex: 1,
      lastMoveDate: new Date(),
      playerIndex: 1,
      players: [
        {
          username: 'alma',
          playerIndex: 1
        },
        {
          username: 'barack',
          playerIndex: 2
        }
      ],
      state: GameState.YourTurn,
      winner: undefined
    },
    {
      id: '2',
      createDate: new Date(),
      currentBoard: {
        gameIdentifier: 1,
        content: undefined
      },
      currentPlayerIndex: 2,
      lastMoveDate: new Date(),
      playerIndex: 1,
      players: [
        {
          username: 'alma',
          playerIndex: 1
        },
        {
          username: 'alma',
          playerIndex: 2
        }
      ],
      state: GameState.EnemyTurn,
      winner: undefined
    }
  ]);

  constructor() { }

  ngOnInit() {
  }
}
