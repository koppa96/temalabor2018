import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-my-lobby',
  templateUrl: './my-lobby.component.html',
  styleUrls: ['./my-lobby.component.scss']
})
export class MyLobbyComponent implements OnInit {
  @Output() lobbyLeave = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onLobbyLeave() {
    this.lobbyLeave.emit();
  }

}
