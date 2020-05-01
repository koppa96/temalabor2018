import { Injectable } from '@angular/core';
import { GameTypeDto, MatchesClient, MatchStatus, RollListDtoOfMatchStatus } from '../../../shared/clients';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { toLocalDate } from '../../../shared/services/date-utils';

@Injectable()
export class MatchService {

  constructor(private matchesClient: MatchesClient) { }

  getCurrentMatches(oldestId: string | null, count: number): Observable<RollListDtoOfMatchStatus> {
    return this.matchesClient.getCurrentMatches(oldestId || '', count).pipe(
      tap(dto => {
        for (const element of dto.data) {
          element.createDate = toLocalDate(element.createDate);
          element.lastMoveDate = toLocalDate(element.lastMoveDate);
        }
      })
    );
  }

  getAvailableGameTypes(): Observable<GameTypeDto[]> {
    return this.matchesClient.getGameTypes();
  }
}
