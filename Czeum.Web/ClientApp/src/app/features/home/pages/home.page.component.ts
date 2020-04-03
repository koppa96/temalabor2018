import { Component, OnInit } from '@angular/core';
import { GameType, StatisticsDto, AchivementDto } from '../../../shared/clients';

@Component({
  templateUrl: './home.page.component.html',
  styleUrls: ['./home.page.component.scss']
})
export class HomePageComponent implements OnInit {
  mockStatistics: StatisticsDto = new StatisticsDto({
    playedGames: 152,
    wonGames: 101,
    favouriteGame: GameType.Chess,
    playedGamesOfFavourite: 43,
    wonGamesOfFavourite: 22,
    mostPlayedWithName: 'Gipsz Jakab'
  });

  mockAchivements: AchivementDto[] = [
    new AchivementDto({
      id: 'a',
      title: 'Connect4 király - 1. szint',
      description: 'Játssz és nyerj 1 Connect4 játékot!',
      isStarred: true,
      unlockedAt: new Date()
    }),
    new AchivementDto({
      id: 'b',
      title: 'Susztermatt',
      description: 'Adj mattot valakinek a lehető legkevesebb lépésből',
      isStarred: true,
      unlockedAt: new Date(2020, 1, 12)
    }),
    new AchivementDto({
      id: 'c',
      title: 'Sakk király - 2. szint',
      description: 'Játssz és nyerj 25 sakk játékot!',
      isStarred: false,
      unlockedAt: new Date(2020, 2, 14)
    }),
    new AchivementDto({
      id: 'd',
      title: 'Gyorsjáték bajnok',
      description: 'Nyerj 25 gyors játékot!',
      isStarred: false,
      unlockedAt: new Date(2020, 2, 2)
    })
  ];

  constructor() { }

  ngOnInit() {
  }

}
