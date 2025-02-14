import {AfterViewInit, ChangeDetectorRef, Component, inject, OnInit, PLATFORM_ID} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {User} from '../../../shared/models/user';
import {AuthService} from '../../services/auth.service';
import {Router} from '@angular/router';
import {ThemeService} from '../../../shared/services/theme.service';
import {ToastrService} from 'ngx-toastr';
import {isPlatformBrowser} from '@angular/common';

declare var bootstrap: any;

@Component({
  selector: 'app-login-modal',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './login-modal.component.html',
  styleUrls: [
    './login-modal.component.css',
    '../../layout/auth/auth.component.css'
  ]
})
export class LoginModalComponent implements OnInit, AfterViewInit {
  isSubmitted: boolean = false;
  isLoading: boolean = false;
  user: User | null = null;
  platForm = inject(PLATFORM_ID);

  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private router: Router,
    private themeService: ThemeService,
    private cdr: ChangeDetectorRef,
    private toast: ToastrService) {
  }

  ngAfterViewInit(): void {
    //this.service.deleteToken();
  }

  form = this.formBuilder.group({
    userName: ['', Validators.required],
    password: ['', Validators.required],
    rememberMe: [false]
  })

  ngOnInit() {
    if (isPlatformBrowser(this.platForm)) {
      this.themeService.theme$.subscribe(theme => {
        document.body.className = theme;
        this.cdr.detectChanges();
      });
      this.togglePasswordVisibility();
    }
  }

  hasDisplayErrors(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty));
  }

  onSubmit() {

    const currentUrl = this.router.url;
    this.isSubmitted = true;
    if (this.form.valid) {
      this.isLoading = true;
      this.service.login(this.form.value).subscribe({
        next: (res: any) => {
          this.isLoading = false;
          console.log(res.data)
          const {accessToken, refreshToken, clientId, refreshTokenExpiry} = res.data;
          this.service.saveToken(accessToken, refreshToken, clientId, refreshTokenExpiry);
          this.toast.success('Login successful', 'Success');
          if (isPlatformBrowser(this.platForm)) {
            const modalElement = document.getElementById('loginModal');
            if (modalElement) {
              const modalInstance = bootstrap.Modal.getInstance(modalElement) || new bootstrap.Modal(modalElement);
              modalInstance.hide();
            }
          }

          this.form.reset();
          this.cdr.detectChanges();
          try {
            this.router.navigate([currentUrl]).then(() => {
              window.location.reload();
            });
          } catch (e) {
            this.router.navigate(['/']).then(() => {
              window.location.reload();
            });
          }
        },
        error: err => {
          this.isLoading = false;
          if (err.status === 400) {
            this.toast.error('Invalid username or password', 'Login Failed');
          } else {
            this.toast.error(err.error.message, 'Error');
          }
        }
      });
    }
  }

  togglePasswordVisibility() {
    const togglePassword = document.querySelector('#togglePassword');
    const password = document.querySelector('#password') as HTMLInputElement;
    const modalElement = document.getElementById('loginModal');

    if (modalElement) {
      modalElement.addEventListener('hide.bs.modal', () => {
        this.form.reset();
        this.isSubmitted = false;
      });
    }

    if (togglePassword) {
      togglePassword.addEventListener('click', (e) => {
        e.preventDefault();
        // Toggle the type attribute
        const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
        password.setAttribute('type', type);

        // Toggle the icon
        togglePassword.textContent = type === 'password' ? 'Hiển thị' : 'Ẩn đi';
      });

    }
  }
}
