import { Injectable } from '@angular/core';
import { MatchesClient, MatchStatus, RollListDtoOfMatchStatus } from '../../../shared/clients';
import { Observable } from 'rxjs';

@Injectable()
export class MatchService {

  constructor(private api: MatchesClient) { }

  getCurrentMatches(oldestId: string | null, count: number): Observable<RollListDtoOfMatchStatus> {
    return this.api.getCurrentMatches(oldestId || '', count);
  }
}
