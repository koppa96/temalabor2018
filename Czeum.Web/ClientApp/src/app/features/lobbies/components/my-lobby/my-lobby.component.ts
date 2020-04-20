import { AfterViewInit, ChangeDetectorRef, Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { combineLatest, Observable, Subscription } from 'rxjs';
import { GameTypeDto, LobbyDataWrapper } from '../../../../shared/clients';
import { LobbyAccessDropdownItem, lobbyAccessDropdownItems } from '../../models/lobby-create.models';
import { map, take } from 'rxjs/operators';
import { AuthService } from '../../../../authentication/services/auth.service';
import { Store } from '@ngrx/store';
import { State } from '../../../../reducers';
import { LobbyService } from '../../services/lobby.service';
import { updateLobby } from '../../../../reducers/current-lobby/current-lobby-actions';

@Component({
  selector: 'app-my-lobby',
  templateUrl: './my-lobby.component.html',
  styleUrls: ['./my-lobby.component.scss']
})
export class MyLobbyComponent implements OnInit, OnDestroy, AfterViewInit {
  startErrorMessage: string;
  currentLobby: Observable<LobbyDataWrapper>;
  @Output() lobbyLeave = new EventEmitter();
  @Output() gameStart = new EventEmitter();
  @Output() saveChanges = new EventEmitter<LobbyDataWrapper>();

  currentLobbyName: string;
  lobbyAccesses = lobbyAccessDropdownItems;
  currentLobbyAccess: LobbyAccessDropdownItem;
  currentGameTypeName: string;

  subscription: Subscription;

  constructor(
    private authService: AuthService,
    private store: Store<State>,
    private lobbyService: LobbyService,
    private changeDetectorRef: ChangeDetectorRef
  ) {
    this.currentLobby = this.store.select(x => x.currentLobby);
  }

  ngOnInit() {
    this.lobbyService.getAvailableGameTypes().subscribe(res => {
      this.currentLobby.pipe(
        take(1)
      ).subscribe(lobby => {
        this.currentGameTypeName = res.find(x => x.identifier === lobby.gameIdentifier).displayName;
      });
    });

    this.subscription = this.currentLobby.subscribe(res => {
      if (res) {
        this.currentLobbyAccess = this.lobbyAccesses.find(x => x.lobbyAccess === res.content.access);
        this.currentLobbyName = res.content.name;
      }
    });
  }

  ngAfterViewInit() {
    // Fix ExpressionChangedAfterItHasBeenCheckedError
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  onLobbyLeave() {
    this.lobbyLeave.emit();
  }

  onGameStart() {
    this.gameStart.emit();
  }

  isHost(): Observable<boolean> {
    return combineLatest(this.authService.getAuthState(), this.currentLobby).pipe(
      map(([authState, lobby]) => {
        return !!lobby && lobby.content.host === authState.profile.userName;
      })
    );
  }

  canStart(): Observable<boolean> {
    return combineLatest(this.isHost(), this.currentLobby).pipe(
      map(([isHost, lobby]) => {
        const playerCount = lobby.content.guests.length + 1;
        const minPlayerCount: number = (lobby.content as any).minimumPlayerCount;
        const maxPlayerCount: number = (lobby.content as any).maxiumumPlayerCount;
        if (playerCount < minPlayerCount) {
          this.startErrorMessage = 'Nincs elég játékos a szobában ehhez a játéktípushoz!';
          return false;
        } else if (playerCount > maxPlayerCount) {
          this.startErrorMessage = 'Túl sok játékos van a szobában ehhez a játéktípushoz!';
          return false;
        } else {
          this.startErrorMessage = '';
          return isHost;
        }
      })
    );
  }

  onSaveChanges() {
    this.currentLobby.pipe(
      take(1)
    ).subscribe(lobby => {
      lobby.content.name = this.currentLobbyName;
      lobby.content.access = this.currentLobbyAccess.lobbyAccess;
      this.saveChanges.emit(lobby);
    });
  }

}
