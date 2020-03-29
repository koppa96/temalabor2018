import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/authService';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-signout',
  templateUrl: './signout.component.html',
  styleUrls: ['./signout.component.scss']
})
export class SignoutComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    this.authService.onPostLogout();
    this.router.navigate(['/welcome']);
  }

}
