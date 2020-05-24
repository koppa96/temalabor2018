import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { Observable } from 'rxjs';
import { AchivementDto, StatisticsDto } from '../../../../shared/clients';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  @Input() statistics: StatisticsDto;
  @Input() achivements: AchivementDto[];
  @Output() achivementStarred = new EventEmitter<AchivementDto>();
  @Output() achivementUnstarred = new EventEmitter<AchivementDto>();

  username = 'Példa Pál';
  isQueuing: Observable<boolean>;

  constructor(private store: Store<State>) {
    this.isQueuing = this.store.select(x => x.isQueueing);
  }

  ngOnInit() {
  }



}
