import {ChangeDetectorRef, Component, inject, PLATFORM_ID} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import {AuthService} from '../../services/auth.service';
import {ThemeService} from '../../../shared/services/theme.service';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {CommonModule, isPlatformBrowser} from '@angular/common';
import {Router, RouterLink} from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    CommonModule
  ],
  templateUrl: './login.component.html',
  styleUrls: [
    './login.component.css',
    '../../layout/auth/auth.component.css'
  ]
})
export class LoginComponent {
  isSubmitted: boolean = false;
  isLoading: boolean = false;

  constructor(private toast: ToastrService,
              private service: AuthService,
              private cdr: ChangeDetectorRef,
              private themeService: ThemeService,
              private router: Router,
              public formBuilder: FormBuilder) {
    this.service.deleteToken();
  }

  form = this.formBuilder.group({
    userName: ['', Validators.required],
    password: ['', Validators.required],
    rememberMe: [false]
  })

  ngOnInit() {
    this.themeService.theme$.subscribe(theme => {
      document.body.className = theme;
      this.cdr.detectChanges();
    });
  }

  hasDisplayErrors(controlName: string): Boolean {
    const control = this.form.get(controlName);
    return Boolean(control?.invalid) && (this.isSubmitted || Boolean(control?.touched) || Boolean(control?.dirty));
  }

  onSubmit() {
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
}
