import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/authService';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.scss']
})
export class SigninComponent implements OnInit {

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit() {
    const authCode = this.route.snapshot.queryParams.code;
    this.authService.onAuthCodeReceived(authCode, false).then(
      () => this.router.navigate(['/home'])
    );
  }

}
