import { Injectable } from '@angular/core';
import { StatisticsClient, StatisticsDto } from 'src/app/shared/clients';
import { Observable } from 'rxjs';

@Injectable()
export class StatisticsService {

  constructor(private statisticsClient: StatisticsClient) { }

  getStatistics(): Observable<StatisticsDto> {
    return this.statisticsClient.getStatistics();
  }

}
