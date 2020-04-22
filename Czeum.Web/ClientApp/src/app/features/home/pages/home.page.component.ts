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
  achivements$: Observable<AchivementDto[]>;

  constructor(private statisticsService: StatisticsService) { }

  ngOnInit() {
    this.statistics$ = this.statisticsService.getStatistics();
    this.achivements$ = this.statisticsService.getAchivements();
  }

}
