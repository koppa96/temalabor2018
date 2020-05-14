import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { GameEndDialogData } from '../../models/dialog.models';
import { GameState } from '../../clients';

@Component({
  selector: 'app-game-end-dialog',
  templateUrl: './game-end-dialog.component.html',
  styleUrls: ['./game-end-dialog.component.scss']
})
export class GameEndDialogComponent implements OnInit {
  gameStates = GameState;

  constructor(
    private dialogRef: MatDialogRef<GameEndDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: GameEndDialogData
  ) { }

  ngOnInit() {
  }

  close() {
    this.dialogRef.close();
  }
}
