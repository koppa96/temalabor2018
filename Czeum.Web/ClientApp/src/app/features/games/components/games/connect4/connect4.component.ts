import { ChangeDetectorRef, Component, ElementRef, HostListener, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatchService } from '../../../services/match.service';
import { ActivatedRoute } from '@angular/router';
import { MatchStatus } from '../models/common.models';
import { Connect4BoardData, Connect4Item } from '../models/connect4.models';
import { ObservableHub } from '../../../../../shared/services/observable-hub.service';
import { Subscription } from 'rxjs';
import { GameState } from '../../../../../shared/clients';
import { MatDialog, MatSnackBar } from '@angular/material';
import { GameEndDialogComponent } from '../../../../../shared/components/game-end-dialog/game-end-dialog.component';

@Component({
  selector: 'app-connect4',
  templateUrl: './connect4.component.html',
  styleUrls: ['./connect4.component.scss']
})
export class Connect4Component implements OnInit, OnDestroy {
  private verticalPaddingAndBorder = 6;
  private horizontalPaddingAndBorder = 36;
  private subscription = new Subscription();

  matchStatus: MatchStatus<Connect4BoardData>;
  cellSize: number;
  columnHover: boolean[] = [];
  connect4Items = Connect4Item;

  @ViewChild('boardContainer', { static: false }) boardContainer: ElementRef<HTMLDivElement>;

  constructor(
    private matchService: MatchService,
    private route: ActivatedRoute,
    private changeDetectorRef: ChangeDetectorRef,
    private observableHub: ObservableHub,
    private snackBar: MatSnackBar,
    private dialog: MatDialog
  ) { }

  ngOnInit() {
    const matchId = this.route.parent.snapshot.params.matchId;
    this.matchService.getMatch(matchId).subscribe(result => {
      this.matchStatus = result as any;
      this.columnHover = this.matchStatus.currentBoard.content.board[0].map(x => false);
      this.calculateCellSize();

      this.subscription.add(this.observableHub.receiveResult.subscribe(moveResult => {
        if (moveResult.id === this.matchStatus.id) {
          this.matchStatus = moveResult as any;
          this.showGameEndDialogIfNeeded();
        }
      }));
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
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

  columnClicked(column: number) {
    if (this.matchStatus.state === GameState.YourTurn) {
      this.matchService.sendMove(this.matchStatus.id, this.matchStatus.currentBoard.gameIdentifier, {
        matchId: this.matchStatus.id,
        column
      }).subscribe(result => {
        this.matchStatus = result as any;
        this.showGameEndDialogIfNeeded();
      });
    } else {
      this.snackBar.open(
        'Nem te következel!',
        'BEZÁR',
        {
          duration: 5000,
          panelClass: [ 'snackbar' ]
        }
      );
    }
  }

  @HostListener('window:resize')
  onResize() {
    this.calculateCellSize();
  }

  calculateCellSize() {
    if (this.matchStatus) {
      const cellHeight = (this.boardContainer.nativeElement.clientHeight - this.verticalPaddingAndBorder) /
        this.matchStatus.currentBoard.content.board.length;
      const cellWidth = (this.boardContainer.nativeElement.clientWidth - this.horizontalPaddingAndBorder) /
        this.matchStatus.currentBoard.content.board[0].length;

      this.cellSize = Math.min(cellHeight, cellWidth);
      this.changeDetectorRef.detectChanges();
    }
  }

  onMouseEnter(column: number) {
    if (this.matchStatus.state === GameState.YourTurn) {
      this.columnHover[column] = true;
    }
  }

  omMouseLeave(column: number) {
    this.columnHover[column] = false;
  }
}
