import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { AuthState, State } from '../../../../reducers';
import { combineLatest, Observable } from 'rxjs';
import { LobbySettings, OriginalSettingsValues } from '../../models/lobby-settings.models';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-game-settings',
  templateUrl: './game-settings.component.html',
  styleUrls: ['./game-settings.component.scss']
})
export class GameSettingsComponent implements OnInit {
  settings$: Observable<LobbySettings>;
  originalSettings: OriginalSettingsValues;

  constructor(private store: Store<State>) {
    this.settings$ = this.store.select(x => x.currentLobby).pipe(
      map(lobby => {
        if (lobby) {
          return (lobby.content as any).settings;
        }
        return null;
      })
    );
  }

  ngOnInit() {
    this.settings$.subscribe(res => {
      this.originalSettings = {};
      if (res) {
        const keys = Object.keys(res);
        for (const key of keys) {
          this.originalSettings[key] = res[key].value;
        }
      }
    });
  }

  isHost(): Observable<boolean> {
    return combineLatest(this.store.select(x => x.authState), this.store.select(x => x.currentLobby)).pipe(
      map(([authState, lobby]) => {
        return !!lobby && lobby.content.host === authState.profile.userName;
      })
    );
  }

}
