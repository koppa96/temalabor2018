import { Component, Input, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { Observable } from 'rxjs';
import { joinSoloQueue, leaveSoloQueue } from '../../../../reducers/solo-queue/solo-queue-actions';
import { take } from 'rxjs/operators';
import { AchivementDto, StatisticsDto } from '../../../../shared/clients';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  @Input() statistics: StatisticsDto;
  @Input() achivements: AchivementDto[];

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
