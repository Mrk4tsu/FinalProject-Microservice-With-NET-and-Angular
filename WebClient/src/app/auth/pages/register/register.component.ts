import {ChangeDetectorRef, Component} from '@angular/core';
import {catchError, throwError} from 'rxjs';
import {AbstractControl, FormBuilder, ReactiveFormsModule, ValidatorFn, Validators} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';
import {Router} from '@angular/router';
import {AuthService} from '../../services/auth.service';
import {FirstkeyPipe} from '../../services/helper/firstkey.pipe';
import {CommonModule} from '@angular/common';
import {ThemeService} from '../../../shared/services/theme.service';

@Component({
  selector: 'app-register',
  imports: [
    FirstkeyPipe,
    CommonModule,
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrls: [
    './register.component.css',
    '../../layout/auth/auth.component.css'
  ]
})
export class RegisterComponent {
  isSubmitted: boolean = false;
  isLoading: boolean = false;
  successMessage: string | null = null;
  countdown: number = 0;

  constructor(public formBuilder: FormBuilder,
              private service: AuthService,
              private router: Router,
              private themeService: ThemeService,
              private cdr: ChangeDetectorRef,
              private toast: ToastrService) {
  }

  form = this.formBuilder.group({
    userName: ['', Validators.required],
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [
      Validators.required,
      Validators.minLength(6),
      Validators.pattern(/(?=.*[^a-zA-Z0-9 ])/),
    ]],
    confirmPassword: ['', Validators.required],
  }, {validators: this.passwordMatchValidator()});

  ngOnInit() {
    this.themeService.theme$.subscribe(theme => {
      document.body.className = theme;
      this.cdr.detectChanges();
    });
  }

  passwordMatchValidator(): ValidatorFn {
    return (control: AbstractControl): null => {
      const password = control.get('password');
      const confirmPassword = control.get('confirmPassword');
      if (password && confirmPassword && password.value !== confirmPassword.value) {
        confirmPassword?.setErrors({passwordMismatch: true});
      } else {
        confirmPassword?.setErrors(null);
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
    if (this.form.valid) {
      this.isLoading = true;
      this.service.register(this.form.value).pipe(
        catchError((err) => {
          this.toast.error(err.error.message, 'Error');
          this.isLoading = false;
          return throwError(() => err);
        })
      ).subscribe({
        next: (res: any) => {
          if (res.success) {
            this.form.reset();
            this.isSubmitted = false;
            this.successMessage = 'Đăng kí thành công!';
            this.toast.success(this.successMessage, 'Success');
            this.countdown = 5;
            const interval = setInterval(() => {
              this.countdown--;
              if (this.countdown === 0) {
                clearInterval(interval);
                this.toast.clear();
                this.router.navigate(['/login']).then();
              }
            }, 1000);

            this.isLoading = false;
            this.toast.success('Account created successfully', 'Success');
          }
        }
      });
    }
  }
}
