import { Component, Input, OnInit } from '@angular/core';
import { GameState, GameType, IMatchStatus, MoveResultWrapper, Player } from '../../../../shared/clients';
import { RollList } from '../../../../shared/models/roll-list';

@Component({
  selector: 'app-current-game-list',
  templateUrl: './current-game-list.component.html',
  styleUrls: ['./current-game-list.component.scss']
})
export class CurrentGameListComponent implements OnInit {
  @Input() matches: RollList<IMatchStatus> = new RollList<IMatchStatus>([
    {
      id: '1',
      createDate: new Date(),
      currentBoard: new MoveResultWrapper({
        gameType: GameType.Connect4,
        content: undefined
      }),
      currentPlayerIndex: 1,
      lastMoveDate: new Date(),
      playerIndex: 1,
      players: [
        new Player({
          username: 'alma',
          playerIndex: 1
        }),
        new Player({
          username: 'barack',
          playerIndex: 2
        })
      ],
      state: GameState.YourTurn,
      winner: undefined
    },
    {
      id: '2',
      createDate: new Date(),
      currentBoard: new MoveResultWrapper({
        gameType: GameType.Chess,
        content: undefined
      }),
      currentPlayerIndex: 2,
      lastMoveDate: new Date(),
      playerIndex: 1,
      players: [
        new Player({
          username: 'alma',
          playerIndex: 1
        }),
        new Player({
          username: 'alma',
          playerIndex: 2
        })
      ],
      state: GameState.EnemyTurn,
      winner: undefined
    }
  ]);

  constructor() { }

  ngOnInit() {
  }
}
