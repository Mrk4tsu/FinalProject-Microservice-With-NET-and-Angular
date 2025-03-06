import {ChangeDetectorRef, Component} from '@angular/core';
import {AuthService} from '../../services/auth.service';
import {Router, RouterLink} from '@angular/router';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {CommonModule} from '@angular/common';
import {ToastrService} from 'ngx-toastr';
import {ModalService} from '../../../shared/services/modal.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: [
    './login.component.css',
    '../../layout/auth/auth.component.css'
  ]
})
export class LoginComponent {
  isSubmitted: boolean = false;
  isLoading: boolean = false;

  constructor(private service: AuthService,
              private toast: ToastrService,
              private cdr: ChangeDetectorRef,
              private modal: ModalService,
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
    this.togglePassword();
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

  togglePassword() {
    const togglePass = document.querySelector('#tgPassword');
    const pass = document.querySelector('#password') as HTMLInputElement;

    if (togglePass) {
      togglePass.addEventListener('click', (e) => {
        e.preventDefault();
        // Toggle the type attribute
        const type = pass.getAttribute('type') === 'password' ? 'text' : 'password';
        pass.setAttribute('type', type);

        // Toggle the icon
        togglePass.textContent = type === 'password' ? 'Hiển thị' : 'Ẩn đi';
      });

    }
  }

  isDevelopment() {
    this.modal.showDialogInfo('Chức năng đang được phát triển');
  }
}
