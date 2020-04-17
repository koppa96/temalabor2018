import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return new Promise<boolean | UrlTree>(resolve => {
      this.authService.getAuthState().pipe(
        take(1)
      ).subscribe(res => {
        if (res.isAuthenticated && res.expires.getTime() > new Date().getTime()) {
          resolve(true);
        } else {
          resolve(this.router.createUrlTree(['/welcome']));
        }
      });
    });
  }

}
