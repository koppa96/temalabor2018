import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { MatchStatus } from '../models/common.models';
import { GameState } from '../../../../../shared/clients';
import { ChessBoardData, Color, PieceType } from '../models/chess.models';
import { MatchService } from '../../../services/match.service';
import { ActivatedRoute } from '@angular/router';
import { ObservableHub } from '../../../../../shared/services/observable-hub.service';
import { Subscription } from 'rxjs';
import { GameEndDialogComponent } from '../../../../../shared/components/game-end-dialog/game-end-dialog.component';
import { MatDialog, MatSnackBar } from '@angular/material';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-chess',
  templateUrl: './chess.component.html',
  styleUrls: ['./chess.component.scss']
})
export class ChessComponent implements OnInit, AfterViewInit {
  private verticalPaddingAndBorder = 6;
  private horizontalPaddingAndBorder = 6;

  matchStatus: MatchStatus<ChessBoardData>;
  cellSize: number;
  cellIndices: number[];
  selectedPieceCoordinates: { row: number; column: number };
  isSending = false;

  get currentPlayerColor(): Color {
    console.log(this.matchStatus.playerIndex === 0 ? Color.White : Color.Black);
    return this.matchStatus.playerIndex === 0 ? Color.White : Color.Black;
  }

  subscription = new Subscription();

  @ViewChild('boardContainer', {static: false}) boardContainer: ElementRef<HTMLDivElement>;

  constructor(
    private matchService: MatchService,
    private route: ActivatedRoute,
    private observableHub: ObservableHub,
    private dialog: MatDialog,
    private changeDetectorRef: ChangeDetectorRef,
    private snackBar: MatSnackBar
  ) {
  }

  ngOnInit() {
    const matchId = this.route.parent.snapshot.params.matchId;
    this.matchService.getMatch(matchId).subscribe(result => {
      this.matchStatus = result as any;
      this.cellIndices = this.currentPlayerColor === Color.White ? [0, 1, 2, 3, 4, 5, 6, 7] : [7, 6, 5, 4, 3, 2, 1, 0];

      this.subscription.add(this.observableHub.receiveResult.subscribe(moveResult => {
        this.matchStatus = moveResult as any;
        this.showGameEndDialogIfNeeded();
      }));
    });
  }

  ngAfterViewInit() {
    this.calculateCellSize();
  }

  @HostListener('window:resize')
  onResize() {
    this.calculateCellSize();
  }

  calculateCellSize() {
    const cellHeight = (this.boardContainer.nativeElement.clientHeight - this.verticalPaddingAndBorder) / 8;
    const cellWidth = (this.boardContainer.nativeElement.clientWidth - this.horizontalPaddingAndBorder) / 8;

    console.log({ height: this.boardContainer.nativeElement.clientHeight, width: this.boardContainer.nativeElement.clientWidth});

    this.cellSize = Math.min(cellHeight, cellWidth);
    this.changeDetectorRef.detectChanges();
  }

  showGameEndDialogIfNeeded() {
    if (this.matchStatus.state === GameState.Won ||
      this.matchStatus.state === GameState.Draw ||
      this.matchStatus.state === GameState.Lost) {
      this.dialog.open(GameEndDialogComponent, {
        data: {
          gameState: this.matchStatus.state
        },
        autoFocus: false,
        width: '700px'
      });
    }
  }

  isPieceOnTile(row: number, column: number) {
    return this.matchStatus.currentBoard.content.pieceInfos.some(x => x.row === row && x.column === column);
  }

  getPieceFile(row: number, column: number) {
    const piece = this.matchStatus.currentBoard.content.pieceInfos.find(x => x.row === row && x.column === column);
    let route = 'assets/' + (piece.color === Color.Black ? 'b_' : 'w_');
    switch (piece.type) {
      case PieceType.Bishop:
        route += 'bishop.png';
        break;
      case PieceType.King:
        route += 'king.png';
        break;
      case PieceType.Knight:
        route += 'knight.png';
        break;
      case PieceType.Pawn:
        route += 'pawn.png';
        break;
      case PieceType.Queen:
        route += 'queen.png';
        break;
      case PieceType.Rook:
        route += 'rook.png';
        break;
    }
    return route;
  }

  tileClicked(row: number, column: number) {
    if (this.matchStatus.state === GameState.YourTurn) {
      const piece = this.matchStatus.currentBoard.content.pieceInfos.find(x => x.row === row && x.column === column);
      if (piece) {
        if (piece.color === this.currentPlayerColor) {
          this.selectedPieceCoordinates = {row: piece.row, column: piece.column};
        } else if (this.selectedPieceCoordinates) {
          this.sendMove(row, column);
          this.selectedPieceCoordinates = null;
        } else {
          this.snackBar.open(
            'Válassz ki az egyik saját bábudat a lépéshez!',
            'BEZÁR',
            {
              duration: 5000,
              panelClass: ['snackbar']
            }
          );
        }
      } else {
        if (this.selectedPieceCoordinates) {
          this.sendMove(row, column);
          this.selectedPieceCoordinates = null;
        } else {
          this.snackBar.open(
            'Válassz ki az egyik saját bábudat a lépéshez!',
            'BEZÁR',
            {
              duration: 5000,
              panelClass: ['snackbar']
            }
          );
        }
      }
    } else {
      this.snackBar.open(
        'Nem te következel!',
        'BEZÁR',
        {
          duration: 5000,
          panelClass: ['snackbar']
        }
      );
    }
  }

  sendMove(row: number, column: number) {
    this.isSending = true;
    this.matchService.sendMove(this.matchStatus.id, this.matchStatus.currentBoard.gameIdentifier, {
      matchId: this.matchStatus.id,
      fromRow: this.selectedPieceCoordinates.row,
      fromColumn: this.selectedPieceCoordinates.column,
      toRow: row,
      toColumn: column
    }).pipe(
      finalize(() => this.isSending = false)
    ).subscribe(result => {
      this.matchStatus = result as any;
      this.showGameEndDialogIfNeeded();
    });
  }
}
