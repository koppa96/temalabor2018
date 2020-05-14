import { GameState } from '../clients';

export interface ConfirmDialogData {
  title: string;
  text: string;
}

export interface ConfirmDialogResult {
  shouldProceed: boolean;
}

export interface GameEndDialogData {
  gameState: GameState;
}
