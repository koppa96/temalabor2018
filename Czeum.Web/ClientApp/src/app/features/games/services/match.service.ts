import { Injectable } from '@angular/core';
import {
  GameTypeDto,
  MatchesClient,
  MatchMessagesClient,
  MatchStatus,
  Message,
  MoveData, MoveDataWrapper,
  RollListDtoOfMatchStatus
} from '../../../shared/clients';
import { Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { toLocalDate } from '../../../shared/services/date-utils';
import { RollList } from '../../../shared/models/roll-list';

@Injectable()
export class MatchService {

  constructor(
    private matchesClient: MatchesClient,
    private matchMessagesClient: MatchMessagesClient
  ) { }

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

  getPreviousMatches(oldestId: string | null, count: number): Observable<RollListDtoOfMatchStatus> {
    return this.matchesClient.getFinishedMatches(oldestId || '', count).pipe(
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

  getMatch(matchId: string): Observable<MatchStatus> {
    return this.matchesClient.getMatch(matchId);
  }

  getMatchMessages(matchId: string, count = 25, oldestId?: string): Observable<RollList<Message>> {
    return this.matchMessagesClient.getMessages(matchId, oldestId || '', count).pipe(
      map(dto => new RollList<Message>(dto.data, dto.hasMoreLeft))
    );
  }

  sendMatchMessage(matchId: string, text: string): Observable<Message> {
    return this.matchMessagesClient.sendMessage(matchId, text);
  }

  sendMove(matchId: string, gameIdentifier: number, moveData: any): Observable<MatchStatus> {
    return this.matchesClient.move({
      gameIdentifier,
      content: moveData
    });
  }
}
