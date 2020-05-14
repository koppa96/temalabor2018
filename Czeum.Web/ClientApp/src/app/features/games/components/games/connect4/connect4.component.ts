import { AfterViewInit, ChangeDetectorRef, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { MatchService } from '../../../services/match.service';
import { ActivatedRoute } from '@angular/router';
import { MatchStatus } from '../models/common.models';
import { Connect4BoardData, Connect4Item } from '../models/connect4.models';

@Component({
  selector: 'app-connect4',
  templateUrl: './connect4.component.html',
  styleUrls: ['./connect4.component.scss']
})
export class Connect4Component implements OnInit, AfterViewInit {
  matchStatus: MatchStatus<Connect4BoardData>;
  cellSize: number;

  @ViewChild('boardContainer', { static: false }) boardContainer: ElementRef<HTMLDivElement>;

  constructor(
    private matchService: MatchService,
    private route: ActivatedRoute,
    private changeDetectorRef: ChangeDetectorRef
  ) { }

  ngOnInit() {
    const matchId = this.route.parent.snapshot.params.matchId;
    this.matchService.getMatch(matchId).subscribe(result => {
      this.matchStatus = result as any;
      this.calculateCellSize();
    });
  }

  ngAfterViewInit() {
  }

  @HostListener('window:resize')
  onResize() {
    this.calculateCellSize();
  }

  calculateCellSize() {
    if (this.matchStatus) {
      const cellHeight = this.boardContainer.nativeElement.clientHeight / this.matchStatus.currentBoard.content.board.length;
      const cellWidth = this.boardContainer.nativeElement.clientWidth / this.matchStatus.currentBoard.content.board[0].length;

      this.cellSize = Math.min(cellHeight, cellWidth);
      this.changeDetectorRef.detectChanges();
    }
  }
}
