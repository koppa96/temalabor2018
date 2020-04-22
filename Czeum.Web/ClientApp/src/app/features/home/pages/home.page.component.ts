import { Component, OnInit } from '@angular/core';
import { StatisticsDto, AchivementDto, GameTypeDto } from '../../../shared/clients';
import { StatisticsService } from '../services/statistics.service';
import { Observable } from 'rxjs';

@Component({
  templateUrl: './home.page.component.html',
  styleUrls: ['./home.page.component.scss']
})
export class HomePageComponent implements OnInit {
  statistics$: Observable<StatisticsDto>;

  mockAchivements: AchivementDto[] = [
    {
      id: 'a',
      title: 'Connect4 király - 1. szint',
      description: 'Játssz és nyerj 1 Connect4 játékot!',
      isStarred: true,
      unlockedAt: new Date()
    },
    {
      id: 'b',
      title: 'Susztermatt',
      description: 'Adj mattot valakinek a lehető legkevesebb lépésből',
      isStarred: true,
      unlockedAt: new Date(2020, 1, 12)
    },
    {
      id: 'c',
      title: 'Sakk király - 2. szint',
      description: 'Játssz és nyerj 25 sakk játékot!',
      isStarred: false,
      unlockedAt: new Date(2020, 2, 14)
    },
    {
      id: 'd',
      title: 'Gyorsjáték bajnok',
      description: 'Nyerj 25 gyors játékot!',
      isStarred: false,
      unlockedAt: new Date(2020, 2, 2)
    }
  ];

  constructor(private statisticsService: StatisticsService) { }

  ngOnInit() {
    this.statistics$ = this.statisticsService.getStatistics();
  }

}
