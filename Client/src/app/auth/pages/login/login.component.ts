import {ChangeDetectorRef, Component, inject, PLATFORM_ID} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../service/auth.service';
import {ToastrService} from 'ngx-toastr';
import {Router} from '@angular/router';
import {ThemeService} from '../../../shared/services/theme.service';
import {isPlatformBrowser} from '@angular/common';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  isSubmitted: boolean = false;
  isLoading: boolean = false;
  platForm = inject(PLATFORM_ID);

  constructor(private toast: ToastrService,
              private service: AuthService,
              private router: Router,
              private cdr: ChangeDetectorRef,
              private themeService: ThemeService,
              public formBuilder: FormBuilder) {
    this.service.deleteToken();
  }

  ngOnInit() {
    if (isPlatformBrowser(this.platForm)) {
      this.themeService.theme$.subscribe(theme => {
        document.body.className = theme;
        this.cdr.detectChanges();
      });
    }
  }

  form = this.formBuilder.group({
    userName: ['', Validators.required],
    password: ['', Validators.required],
    rememberMe: [false]
  })

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
