import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatInputModule } from '@angular/material/input';
import { MatListModule } from '@angular/material/list';
import { MatSelectModule } from '@angular/material/select';
import { MenuComponent } from './components/menu/menu.component';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { NotificationComponent } from './components/notification/notification.component';
import { MatCardModule } from '@angular/material/card';
import { FlexModule } from '@angular/flex-layout';
import { FriendsListComponent } from './components/friends-list/friends-list.component';
import { NotificationListItemComponent } from './components/notification-list-item/notification-list-item.component';
import { FriendListItemComponent } from './components/friend-list-item/friend-list-item.component';
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatRippleModule, MatSnackBarModule, MatTooltipModule } from '@angular/material';
import { MatMenuModule } from '@angular/material/menu';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ChatMessageComponent } from './components/chat-message/chat-message.component';
import { PlayerComponent } from './components/player/player.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';

@NgModule({
  declarations: [
    MenuComponent,
    NotificationComponent,
    FriendsListComponent,
    NotificationListItemComponent,
    FriendListItemComponent,
    ChatMessageComponent,
    PlayerComponent,
    ConfirmDialogComponent
  ],
  imports: [
    CommonModule,
    MatButtonModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatInputModule,
    MatListModule,
    MatSelectModule,
    MatIconModule,
    MatCardModule,
    RouterModule,
    FlexModule,
    PerfectScrollbarModule,
    MatProgressSpinnerModule,
    MatRippleModule,
    MatMenuModule,
    MatButtonToggleModule,
    FontAwesomeModule,
    MatTooltipModule,
    MatSnackBarModule,
    MatDialogModule
  ],
  exports: [
    MatButtonModule,
    MatFormFieldModule,
    MatCheckboxModule,
    MatInputModule,
    MatListModule,
    MatSelectModule,
    MenuComponent,
    NotificationComponent,
    MatIconModule,
    MatCardModule,
    FlexModule,
    FriendsListComponent,
    PerfectScrollbarModule,
    MatProgressSpinnerModule,
    MatRippleModule,
    MatMenuModule,
    MatButtonToggleModule,
    FontAwesomeModule,
    MatTooltipModule,
    ChatMessageComponent,
    MatSnackBarModule,
    PlayerComponent,
    MatDialogModule
  ],
  entryComponents: [
    ConfirmDialogComponent
  ]
})
export class SharedModule { }
