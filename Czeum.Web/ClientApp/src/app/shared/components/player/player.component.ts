import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.scss']
})
export class PlayerComponent implements OnInit {
  @Input() username: string;
  @Input() isHost: boolean;
  @Input() iconSize = 120;

  math = Math;

  constructor() { }

  ngOnInit() {
  }

}
