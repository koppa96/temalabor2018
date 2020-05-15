import { Component, Input, OnInit, TemplateRef } from '@angular/core';
import { GameState, MatchStatus } from '../../../../../shared/clients';
import { GameEndDialogComponent } from '../../../../../shared/components/game-end-dialog/game-end-dialog.component';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-game-details',
  templateUrl: './game-details.component.html',
  styleUrls: ['./game-details.component.scss']
})
export class GameDetailsComponent implements OnInit {
  @Input() gameTypeName: string;
  @Input() matchStatus: MatchStatus;
  @Input() description: string;

  gameStates = GameState;

  ngOnInit() {
  }

}
