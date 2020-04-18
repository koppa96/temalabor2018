import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { combineLatest, Observable } from 'rxjs';
import { LobbyDataWrapper } from '../../../../shared/clients';
import { LobbyAccessDropdownItem, lobbyAccessDropdownItems } from '../../models/lobby-create.models';
import { map } from 'rxjs/operators';
import { AuthService } from '../../../../authentication/services/auth.service';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';

@Component({
  selector: 'app-my-lobby',
  templateUrl: './my-lobby.component.html',
  styleUrls: ['./my-lobby.component.scss']
})
export class MyLobbyComponent implements OnInit {
  currentLobby: Observable<LobbyDataWrapper>;
  @Output() lobbyLeave = new EventEmitter();

  currentLobbyName: string;
  lobbyAccesses = lobbyAccessDropdownItems;
  currentLobbyAccess: LobbyAccessDropdownItem;

  constructor(private authService: AuthService, private store: Store<State>) {
    this.currentLobby = this.store.select(x => x.currentLobby);
  }

  ngOnInit() {
    this.currentLobby.subscribe(res => {
      console.log(res);
      if (res) {
        this.currentLobbyAccess = this.lobbyAccesses.find(x => x.lobbyAccess === res.content.access);
        this.currentLobbyName = res.content.name;
      }
    });
  }

  onLobbyLeave() {
    this.lobbyLeave.emit();
  }

  isHost(): Observable<boolean> {
    return combineLatest(this.authService.getAuthState(), this.currentLobby).pipe(
      map(([authState, lobby]) => {
        return !!lobby && lobby.content.host === authState.profile.userName;
      })
    );
  }

}
