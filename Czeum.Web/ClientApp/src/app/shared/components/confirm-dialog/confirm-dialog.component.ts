import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ConfirmDialogData } from '../../models/dialog.models';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})
export class ConfirmDialogComponent implements OnInit {

  constructor(
    private dialogRef: MatDialogRef<ConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogData
  ) { }

  ngOnInit() {
  }

  onCancel() {
    this.dialogRef.close({
      shouldProceed: false
    });
  }

  onProceed() {
    this.dialogRef.close({
      shouldProceed: true
    });
  }

}
