import { AfterViewInit, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { MatchStatus } from '../models/common.models';
import { GameState } from '../../../../../shared/clients';
import { ChessBoardData, Color, PieceType } from '../models/chess.models';
import { MatchService } from '../../../services/match.service';
import { ActivatedRoute } from '@angular/router';
import { ObservableHub } from '../../../../../shared/services/observable-hub.service';
import { Subscription } from 'rxjs';
import { GameEndDialogComponent } from '../../../../../shared/components/game-end-dialog/game-end-dialog.component';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-chess',
  templateUrl: './chess.component.html',
  styleUrls: ['./chess.component.scss']
})
export class ChessComponent implements OnInit, AfterViewInit {
  matchStatus: MatchStatus<ChessBoardData>;
  cellSize: number;
  gameStates = GameState;
  cellIndices = [ 0, 1, 2, 3, 4, 5, 6, 7 ];

  subscription = new Subscription();

  @ViewChild('boardContainer', { static: false }) boardContainer: ElementRef<HTMLDivElement>;

  constructor(
    private matchService: MatchService,
    private route: ActivatedRoute,
    private observableHub: ObservableHub,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    const matchId = this.route.parent.snapshot.params.matchId;
    this.matchService.getMatch(matchId).subscribe(result => {
      this.matchStatus = result as any;

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
    const cellHeight = this.boardContainer.nativeElement.clientHeight / 8;
    const cellWidth = this.boardContainer.nativeElement.clientWidth / 8;

    this.cellSize = Math.min(cellHeight, cellWidth);
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
      case PieceType.Bishop: route += 'bishop.png'; break;
      case PieceType.King: route += 'king.png'; break;
      case PieceType.Knight: route += 'knight.png'; break;
      case PieceType.Pawn: route += 'pawn.png'; break;
      case PieceType.Queen: route += 'queen.png'; break;
      case PieceType.Rook: route += 'rook.png'; break;
    }
    return route;
  }

}
