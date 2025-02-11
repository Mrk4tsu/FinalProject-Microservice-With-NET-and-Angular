import {Component} from '@angular/core';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../service/auth.service';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-email-change',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './email-change.component.html',
  styleUrl: './email-change.component.css'
})
export class EmailChangeComponent {
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
    this.authService.requestEmailChange(this.form.value.newEmail!).subscribe({
      next: () => this.message = 'Vui lòng kiểm tra email để xác nhận',
      error: (err) => this.message = err.error?.message || 'Lỗi hệ thống'
    });
  }
}
