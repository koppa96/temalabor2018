import { Injectable } from '@angular/core';
import { StatisticsClient, StatisticsDto, AchivetmentsClient, AchivementDto } from 'src/app/shared/clients';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { toLocalDate } from '../../../shared/services/date-utils';

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
    return this.achivementsClient.getAchivements().pipe(
      tap(achivements => {
        for (const achivement of achivements) {
          achivement.unlockedAt = toLocalDate(achivement.unlockedAt);
        }
      })
    );
  }

  starAchivement(id: string): Observable<AchivementDto> {
    return this.achivementsClient.starAchivement(id);
  }

  unstarAchivement(id: string): Observable<AchivementDto> {
    return this.achivementsClient.unstarAchivement(id);
  }

}
