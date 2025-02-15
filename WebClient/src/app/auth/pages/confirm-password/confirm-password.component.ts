import {ChangeDetectorRef, Component, inject, OnInit, PLATFORM_ID} from '@angular/core';
import {AuthService} from '../../services/auth.service';
import {AbstractControl, FormBuilder, FormsModule, ReactiveFormsModule, ValidatorFn, Validators} from '@angular/forms';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';
import {CommonModule, isPlatformBrowser} from '@angular/common';
import {FirstkeyPipe} from '../../services/helper/firstkey.pipe';
import {ThemeService} from '../../../shared/services/theme.service';

@Component({
  selector: 'app-confirm-password',
  imports: [
    FormsModule,
    ReactiveFormsModule,
    RouterLink,
    CommonModule,
    FirstkeyPipe
  ],
  templateUrl: './confirm-password.component.html',
  styleUrls: [
    './confirm-password.component.css',
    '../../layout/auth/auth.component.css'
  ]
})
export class ConfirmPasswordComponent implements OnInit {
  authService = inject(AuthService);
  message = '';
  isLoading: boolean = false;
  isSubmitted: boolean = false;
  countdown: number = 0;
  platForm = inject(PLATFORM_ID);
  form = this.fb.group({
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    confirmNewPassword: ['', [Validators.required]]
  }, {validators: this.passwordMatchValidator()});

  constructor(private fb: FormBuilder,
              private themeService: ThemeService,
              private cdr: ChangeDetectorRef,
              private router: Router,
              private route: ActivatedRoute) {
  }

  ngOnInit() {
    if (isPlatformBrowser(this.platForm)) {
      this.themeService.theme$.subscribe(theme => {
        document.body.className = theme;
        this.cdr.detectChanges();
      });
    }
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
