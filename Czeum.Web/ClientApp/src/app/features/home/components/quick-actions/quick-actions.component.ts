import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { take } from 'rxjs/operators';
import { joinSoloQueue, leaveSoloQueue } from '../../../../reducers/solo-queue/solo-queue-actions';

@Component({
  selector: 'app-quick-actions',
  templateUrl: './quick-actions.component.html',
  styleUrls: ['./quick-actions.component.scss']
})
export class QuickActionsComponent implements OnInit {

  username = 'Példa Pál';
  isQueuing: Observable<boolean>;

  constructor(private store: Store<State>) {
    this.isQueuing = this.store.select(x => x.isQueueing);
  }

  ngOnInit() {
  }

  toggleQueuing() {
    this.isQueuing.pipe( take(1) ).subscribe(res => {
      if (res) {
        this.store.dispatch(leaveSoloQueue());
      } else {
        this.store.dispatch(joinSoloQueue());
      }
    });
  }

}
