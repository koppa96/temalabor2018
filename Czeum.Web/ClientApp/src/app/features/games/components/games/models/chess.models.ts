export enum Color {
  Black, White
}

export enum PieceType {
  King, Queen, Bishop, Knight, Rook, Pawn
}

export interface PieceInfo {
  color: Color;
  type: PieceType;
  row: number;
  column: number;
}

export interface ChessBoardData {
  whiteKingInCheck: boolean;
  blackKingInCheck: boolean;
  pieceInfos: PieceInfo[];
}
