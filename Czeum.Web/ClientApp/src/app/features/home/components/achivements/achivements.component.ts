import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AchivementDto } from '../../../../shared/clients';

@Component({
  selector: 'app-achivements',
  templateUrl: './achivements.component.html',
  styleUrls: ['./achivements.component.scss']
})
export class AchivementsComponent implements OnInit {
  @Input() achivements: AchivementDto[];
  @Output() achivementStarred = new EventEmitter<AchivementDto>();
  @Output() achivementUnstarred = new EventEmitter<AchivementDto>();

  constructor() { }

  ngOnInit() {
  }

  achivementStarClicked(achivement: AchivementDto) {
    if (achivement.isStarred) {
      this.achivementUnstarred.emit(achivement);
    } else {
      this.achivementStarred.emit(achivement);
    }
  }
}
