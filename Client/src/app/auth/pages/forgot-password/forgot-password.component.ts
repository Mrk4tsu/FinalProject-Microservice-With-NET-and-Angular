import {Component} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../service/auth.service';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-forgot-password',
  imports: [
    NgIf,
    ReactiveFormsModule
  ],
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.css'
})
export class ForgotPasswordComponent {
  form = this.fb.group({
    newEmail: ['', [Validators.required, Validators.email]]
  });
  message = '';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService
  ) {
  }

  onSubmit() {
    this.authService.requestForgotPassword(this.form.value.newEmail!).subscribe({
      next: () => this.message = 'Vui lòng kiểm tra email để xác nhận',
      error: (err) => this.message = err.error?.message || 'Lỗi hệ thống'
    });
  }
}
