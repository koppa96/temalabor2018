import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { LobbyDataWrapper } from '../../../../shared/clients';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';

@Component({
  selector: 'app-lobby-list',
  templateUrl: './lobby-list.component.html',
  styleUrls: ['./lobby-list.component.scss']
})
export class LobbyListComponent implements OnInit {
  currentLobby$: Observable<LobbyDataWrapper>;

  constructor(store: Store<State>) {
    this.currentLobby$ = store.select(x => x.currentLobby);
  }

  ngOnInit() {
  }

}
