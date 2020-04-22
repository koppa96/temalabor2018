import { Injectable } from '@angular/core';
import { StatisticsClient, StatisticsDto, AchivetmentsClient, AchivementDto } from 'src/app/shared/clients';
import { Observable } from 'rxjs';

@Injectable()
export class StatisticsService {

  constructor(
    private statisticsClient: StatisticsClient,
    private achivementsClient: AchivetmentsClient
  ) { }

  getStatistics(): Observable<StatisticsDto> {
    return this.statisticsClient.getStatistics();
  }

  getAchivements(): Observable<AchivementDto[]> {
    return this.achivementsClient.getAchivements();
  }

  starAchivement(id: string): Observable<AchivementDto> {
    return this.achivementsClient.starAchivement(id);
  }

  unstarAchivement(id: string): Observable<AchivementDto> {
    return this.achivementsClient.unstarAchivement(id);
  }

}
