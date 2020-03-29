import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SigninComponent } from './authentication/components/signin/signin.component';
import { SignoutComponent } from './authentication/components/signout/signout.component';
import { AuthGuard } from './authentication/auth-guard/auth.guard';


const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home'
  },
  {
    path: 'signin-oidc',
    component: SigninComponent
  },
  {
    path: 'signout-oidc',
    component: SignoutComponent
  },
  {
    path: 'home',
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./features/home/home.module').then(m => m.HomeModule)
  },
  {
    path: 'games',
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./features/games/games.module').then(m => m.GamesModule)
  },
  {
    path: 'lobbies',
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./features/lobbies/lobbies.module').then(m => m.LobbiesModule)
  },
  {
    path: 'friends',
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./features/friends/friends.module').then(m => m.FriendsModule)
  },
  {
    path: 'messages',
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./features/messages/messages.module').then(m => m.MessagesModule)
  },
  {
    path: 'settings',
    canActivate: [ AuthGuard ],
    loadChildren: () => import('./features/settings/settings.module').then(m => m.SettingsModule)
  },
  {
    path: 'welcome',
    loadChildren: () => import('./features/welcome/welcome.module').then(m => m.WelcomeModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
