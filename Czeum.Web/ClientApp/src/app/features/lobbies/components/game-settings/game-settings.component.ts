import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthState, State } from '../../../../reducers';
import { combineLatest, Observable } from 'rxjs';
import { LobbySettings } from '../../models/lobby-settings.models';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-game-settings',
  templateUrl: './game-settings.component.html',
  styleUrls: ['./game-settings.component.scss']
})
export class GameSettingsComponent implements OnInit {
  settings$: Observable<LobbySettings>;

  constructor(private store: Store<State>) {
    this.settings$ = this.store.select(x => x.currentLobby).pipe(
      map(lobby => {
        return (lobby.content as any).settings;
      })
    );
  }

  ngOnInit() {
    this.settings$.subscribe(res => console.log(res));
  }

  isHost(): Observable<boolean> {
    return combineLatest(this.store.select(x => x.authState), this.store.select(x => x.currentLobby)).pipe(
      map(([authState, lobby]) => {
        return !!lobby && lobby.content.host === authState.profile.userName;
      })
    );
  }

}
