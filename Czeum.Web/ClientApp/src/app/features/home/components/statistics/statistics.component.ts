import { Component, Input, OnInit } from '@angular/core';
import { GameType, StatisticsDto } from '../../../../shared/clients';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {
  @Input() statistics: StatisticsDto;

  constructor() { }

  ngOnInit() {
  }

  getWinrate(wonCount: number, playedCount): string {
    if (playedCount === 0) {
      return 'Nem számítható';
    }

    return `${(wonCount / playedCount * 100).toFixed(2)}%`;
  }

  getGameName(gameType: GameType): string {
    switch (gameType) {
      case GameType.Chess:
        return 'Sakk';
      case GameType.Connect4:
        return 'Connect4';
    }
  }

}
