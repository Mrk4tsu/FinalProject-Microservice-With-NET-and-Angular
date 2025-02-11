import {Component, inject, OnInit, PLATFORM_ID} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {HttpClient} from '@angular/common/http';
import {isPlatformBrowser} from '@angular/common';
import {AuthService} from '../../service/auth.service';

@Component({
  selector: 'app-confirm-email',
  imports: [],
  templateUrl: './confirm-email.component.html',
  styleUrl: './confirm-email.component.css'
})
export class ConfirmEmailComponent implements OnInit {
  platForm = inject(PLATFORM_ID);
  http = inject(HttpClient);
  route = inject(ActivatedRoute);
  authService = inject(AuthService);

  status: 'loading' | 'success' | 'error' = 'loading';
  errorMessage = '';
  constructor() {
  }

  ngOnInit() {
    if (isPlatformBrowser(this.platForm)) {
      const params = this.route.snapshot.queryParams;
      this.authService.confirmEmailChange(params['userId'], params['newEmail'], params['token']).subscribe({
        next: () => this.status = 'success',
        error: (err) => {
          this.status = 'error';
          this.errorMessage = err.error || 'Lỗi xác nhận email';
        }
      });
    }
  }
}
