import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-in-check-dialog',
  templateUrl: './in-check-dialog.component.html',
  styleUrls: ['./in-check-dialog.component.scss']
})
export class InCheckDialogComponent implements OnInit {

  constructor(private dialogRef: MatDialogRef<InCheckDialogComponent>) { }

  ngOnInit() {
  }

  close() {
    this.dialogRef.close();
  }

}
