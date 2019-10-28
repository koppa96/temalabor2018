import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';
import { ConfirmEmailData } from '../../models/auth-models';
import { delay } from 'rxjs/operators';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css']
})
export class ConfirmEmailComponent implements OnInit {
  check = faCheck;
  fail = faTimes;
  isLoading = false;
  confirmSuccessful = false;
  confirmFailed = false;

  constructor(
    route: ActivatedRoute,
    authService: AuthService
  ) {
    this.isLoading = true;
    route.queryParams.subscribe(
      params => {
        const data: ConfirmEmailData = {
          username: params.username,
          token: params.token
        };

        authService.confirmEmail(data)
          .subscribe(
            () => this.confirmSuccessful = true,
            () => this.confirmFailed = true
          )
          .add(() => this.isLoading = false);
      }
    );
  }

  ngOnInit() {
  }

}
