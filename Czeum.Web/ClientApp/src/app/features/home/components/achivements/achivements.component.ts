import { Component, Input, OnInit } from '@angular/core';
import { AchivementDto } from '../../../../shared/clients';

@Component({
  selector: 'app-achivements',
  templateUrl: './achivements.component.html',
  styleUrls: ['./achivements.component.scss']
})
export class AchivementsComponent implements OnInit {
  @Input() achivements: AchivementDto[];

  constructor() { }

  ngOnInit() {
  }

}
