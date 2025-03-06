import {ChangeDetectorRef, Component} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../services/auth.service';
import {CommonModule} from '@angular/common';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-forgot-password',
  imports: [
    ReactiveFormsModule,
    CommonModule,
    RouterLink],
  templateUrl: './forgot-password.component.html',
  styleUrls: [
    './forgot-password.component.css',
    '../../layout/auth/auth.component.css'
  ]
})
export class ForgotPasswordComponent {
  form = this.fb.group({
    username: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]]
  });
  message = '';
  isLoading: boolean = false;
  isSubmitted: boolean = false;
  isSuccess = false;

  constructor(
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
    private authService: AuthService) {
  }

  hasDisplayError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched));
  }

  isDisableSubmit() {
    return this.isLoading || !this.form.valid;
  }

  onSubmit() {
    this.isSubmitted = true;
    if (this.form.valid) {
      this.isLoading = true;
      this.authService.requestForgotPassword(this.form.value).subscribe({
        next: (res: any) => {
          this.isLoading = false;
          this.isSuccess = true;
          this.message = 'Vui lòng kiểm tra email để xác nhận';
        },
        error: (err: any) => {
          this.isLoading = false;
          this.isSuccess = false;
          this.message = err.error?.message || 'Lỗi hệ thống'
        }
      });
    }
  }
}
