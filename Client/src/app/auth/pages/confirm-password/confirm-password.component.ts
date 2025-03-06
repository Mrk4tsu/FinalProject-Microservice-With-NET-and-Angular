import {ChangeDetectorRef, Component, inject} from '@angular/core';
import {AbstractControl, FormBuilder, ReactiveFormsModule, ValidatorFn, Validators} from '@angular/forms';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {FirstkeyPipe} from '../../services/helper/firstkey.pipe';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-confirm-password',
  imports: [
    ReactiveFormsModule,
    FirstkeyPipe,
    CommonModule,
    RouterLink
  ],
  templateUrl: './confirm-password.component.html',
  styleUrls: [
    './confirm-password.component.css',
    '../../layout/auth/auth.component.css'
  ]
})
export class ConfirmPasswordComponent {
  authService = inject(AuthService);
  message = '';
  isLoading: boolean = false;
  isSubmitted: boolean = false;
  isShowPassword: boolean = false;
  countdown: number = 0;
  form = this.fb.group({
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    confirmNewPassword: ['', [Validators.required]]
  }, {validators: this.passwordMatchValidator()});

  constructor(private fb: FormBuilder,
              private cdr: ChangeDetectorRef,
              private router: Router,
              private route: ActivatedRoute) {
  }

  passwordMatchValidator(): ValidatorFn {
    return (control: AbstractControl): null => {
      const newPassword = control.get('newPassword');
      const confirmNewPassword = control.get('confirmNewPassword');
      if (newPassword && confirmNewPassword && newPassword.value !== confirmNewPassword.value) {
        confirmNewPassword?.setErrors({passwordMismatch: true});
      } else {
        confirmNewPassword?.setErrors(null);
      }
      return null;
    }
  }

  hasDisplayError(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched));
  }

  isDisableSubmit() {
    return this.isLoading || !this.form.valid;
  }

  toggleShowPassword() {
    this.isShowPassword = !this.isShowPassword;
  }

  onSubmit() {
    this.isSubmitted = true;
    const params = this.route.snapshot.queryParams;
    if (this.form.valid) {
      this.isLoading = true;
      this.authService.confirmPassword(params['username'], params['token'], this.form.value).subscribe({
        next: (res: any) => {
          this.isLoading = false;
          this.message = 'Mật khẩu của bạn đã thay đổi thành công';
          this.countdown = 3;
          const interval = setInterval(() => {
            this.countdown--;
            if (this.countdown === 0) {
              clearInterval(interval);
              this.router.navigate(['/login']).then(r => r);
            }
          }, 1000);
        },
        error: (err: any) => {
          this.isLoading = false;
          this.message = err.error?.message || 'Lỗi hệ thống'
        }
      });
    }
  }
}
