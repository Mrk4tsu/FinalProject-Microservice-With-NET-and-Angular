import {Component, inject, OnInit, PLATFORM_ID} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {ActivatedRoute} from '@angular/router';
import {AuthService} from '../../service/auth.service';
import {isPlatformBrowser} from '@angular/common';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';

@Component({
  selector: 'app-confirm-password',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './confirm-password.component.html',
  styleUrl: './confirm-password.component.css'
})
export class ConfirmPasswordComponent implements OnInit {
  platForm = inject(PLATFORM_ID);
  http = inject(HttpClient);
  route = inject(ActivatedRoute);
  authService = inject(AuthService);

  status: 'loading' | 'success' | 'error' = 'loading';
  errorMessage = '';
  form = this.fb.group({
    newPassword: ['', [Validators.required, Validators.minLength(6)]]
  });

  constructor(private fb: FormBuilder) {
  }

  ngOnInit() {
  }

  onSubmit() {
    if (isPlatformBrowser(this.platForm)) {
      const params = this.route.snapshot.queryParams;
      this.authService.confirmPassword(params['username'], params['token'], this.form.value.newPassword!).subscribe({
        next: () => console.log('success'),
        error: (err) => {
          this.status = 'error';
          this.errorMessage = err.error || 'Lỗi xác nhận mật khẩu';
        }
      });
    }
  }
}
