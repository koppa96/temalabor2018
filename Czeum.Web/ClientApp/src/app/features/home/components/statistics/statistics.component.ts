import { Component, Input, OnInit } from '@angular/core';
import { StatisticsDto } from '../../../../shared/clients';

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {
  @Input() statistics: StatisticsDto;

  constructor() { }

  ngOnInit() {
  }

  getWinrate(wonCount: number, playedCount): string {
    if (playedCount === 0) {
      return 'Nem számítható';
    }

    return `${(wonCount / playedCount * 100).toFixed(2)}%`;
  }

}
