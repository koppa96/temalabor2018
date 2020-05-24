import { Component, OnInit } from '@angular/core';
import { StatisticsDto, AchivementDto, GameTypeDto } from '../../../shared/clients';
import { StatisticsService } from '../services/statistics.service';
import { Observable } from 'rxjs';
import { firstBy } from 'thenby';
import { ObservableHub } from '../../../shared/services/observable-hub.service';

@Component({
  templateUrl: './home.page.component.html',
  styleUrls: ['./home.page.component.scss']
})
export class HomePageComponent implements OnInit {
  statistics$: Observable<StatisticsDto>;
  achivements: AchivementDto[] = null;

  constructor(
    private statisticsService: StatisticsService,
    private observableHub: ObservableHub
  ) { }

  ngOnInit() {
    this.statistics$ = this.statisticsService.getStatistics();
    this.statisticsService.getAchivements().subscribe(res => {
      this.achivements = res;
      this.orderAchivements();

      this.observableHub.achivementUnlocked.subscribe(achivement => {
        this.achivements.push(achivement);
        this.orderAchivements();
      });
    });
  }

  achivementStarred(achivement: AchivementDto) {
    this.statisticsService.starAchivement(achivement.id).subscribe(() => {
      achivement.isStarred = true;
      this.orderAchivements();
    });
  }

  achivementUnstarred(achivement: AchivementDto) {
    this.statisticsService.unstarAchivement(achivement.id).subscribe(() => {
      achivement.isStarred = false;
      this.orderAchivements();
    });
  }

  orderAchivements() {
    this.achivements = this.achivements.sort(firstBy<AchivementDto>(x => x.isStarred, 'desc')
      .thenBy<AchivementDto>(x => x.unlockedAt, 'desc'));
  }
}
