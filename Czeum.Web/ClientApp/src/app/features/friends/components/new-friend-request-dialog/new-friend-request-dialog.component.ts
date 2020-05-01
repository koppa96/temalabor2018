import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { NewRequestDialogData } from '../../models/new-request-dialog.models';
import { FormControl } from '@angular/forms';
import { UserDto } from '../../../../shared/clients';

@Component({
  selector: 'app-new-friend-request-dialog',
  templateUrl: './new-friend-request-dialog.component.html',
  styleUrls: ['./new-friend-request-dialog.component.scss']
})
export class NewFriendRequestDialogComponent implements OnInit {
  selectedUser = new FormControl();
  showError = false;
  filteredUsers: UserDto[];
  filterText: string;

  constructor(
    private dialogRef: MatDialogRef<NewFriendRequestDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: NewRequestDialogData
  ) { }

  ngOnInit() {
    this.selectedUser.valueChanges.subscribe(value => {
      this.filterUsers();
    });
    this.filteredUsers = this.data.users;
  }

  displayFn(user: UserDto): string {
    return user ? user.username : '';
  }

  onCancel() {
    this.dialogRef.close();
  }

  canProceed() {
    return this.selectedUser && this.selectedUser.value && this.selectedUser.value.username;
  }

  onProceed() {
    if (!this.selectedUser.value) {
      this.showError = true;
    }

    this.dialogRef.close({
      selectedUser: this.selectedUser.value
    });
  }

  filterUsers() {
    if (this.filterText && this.filterText.toLowerCase()) {
      this.filteredUsers = this.data.users.filter(x => x.username.toLowerCase().includes(this.filterText.toLowerCase()));
    }
  }

  onInput() {
    this.showError = false;
    this.filterUsers();
  }
}
