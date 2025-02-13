import {ChangeDetectorRef, Component, inject, OnInit, PLATFORM_ID} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService, User} from '../../service/auth.service';
import {Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {ThemeService} from '../../../shared/services/theme.service';
import {isPlatformBrowser} from '@angular/common';

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
export class LoginModalComponent implements OnInit {
  platForm = inject(PLATFORM_ID);
  isSubmitted: boolean = false;
  isLoading: boolean = false;
  user: User | null = null;

  constructor(
    public formBuilder: FormBuilder,
    private service: AuthService,
    private router: Router,
    private themeService: ThemeService,
    private cdr: ChangeDetectorRef,
    private toast: ToastrService) {
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

  onLogin() {
    this.isSubmitted = true;
    if (this.form.valid) {
      this.isLoading = true;
      this.service.login(this.form.value).subscribe({
        next: (res) => {
          if (!res.success) {
            this.isLoading = false;
            this.toast.error(res.message);
          } else {
            this.isLoading = false;
            this.toast.success('Login successful');
            this.router.navigate(['/']).then(r => r);
          }
        },
        error: (err) => {
          this.isLoading = false;
          this.toast.error(err.error.message);
        },
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
