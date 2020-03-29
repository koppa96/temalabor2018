import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { AuthState, State } from '../../../../reducers';
import { take } from 'rxjs/operators';
import { joinSoloQueue, leaveSoloQueue } from '../../../../reducers/solo-queue/solo-queue-actions';
import { AuthService } from '../../../../authentication/services/authService';

@Component({
  selector: 'app-quick-actions',
  templateUrl: './quick-actions.component.html',
  styleUrls: ['./quick-actions.component.scss']
})
export class QuickActionsComponent implements OnInit {

  authState$: Observable<AuthState>;
  isQueuing$: Observable<boolean>;

  constructor(private store: Store<State>, private authService: AuthService) {
    this.isQueuing$ = this.store.select(x => x.isQueueing);
    this.authState$ = this.authService.getAuthState();
  }

  ngOnInit() {
  }

  toggleQueuing() {
    this.isQueuing$.pipe( take(1) ).subscribe(res => {
      if (res) {
        this.store.dispatch(leaveSoloQueue());
      } else {
        this.store.dispatch(joinSoloQueue());
      }
    });
  }

}
