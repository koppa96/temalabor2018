import { GameState, Player } from '../../../../../shared/clients';

export interface BoardDataWrapper<TBoardData> {
  gameIdentifier: number;
  content: TBoardData;
}

export interface MatchStatus<TBoardData> {
  id: string;
  currentPlayerIndex: number;
  playerIndex: number;
  players?: Player[] | undefined;
  currentBoard?: BoardDataWrapper<TBoardData>;
  state: GameState;
  winner?: string | undefined;
  createDate: Date;
  lastMoveDate?: Date | undefined;
}
