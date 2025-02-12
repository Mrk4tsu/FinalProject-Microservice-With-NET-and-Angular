import {Component, inject, OnInit, PLATFORM_ID} from '@angular/core';
import {AuthService} from '../../service/auth.service';
import {Router} from '@angular/router';
import {FormBuilder, ReactiveFormsModule, Validators} from '@angular/forms';
import {ModalService} from '../../../shared/component/modal/modal.service';
import {isPlatformBrowser} from '@angular/common';

@Component({
  selector: 'app-change-password',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './change-password.component.html',
  styleUrl: './change-password.component.css'
})
export class ChangePasswordComponent implements OnInit {
  platForm = inject(PLATFORM_ID);
  authService = inject(AuthService);
  router = inject(Router);
  fb = inject(FormBuilder);
  modal = inject(ModalService);
  form = this.fb.group({
    currentPassword: ['', [Validators.required]],
    newPassword: ['', [Validators.required, Validators.minLength(6)]],
    confirmPassword: ['', [Validators.required, Validators.minLength(6)]],
    logoutEverywhere: [false]
  });

  ngOnInit() {
    if (isPlatformBrowser(this.platForm)) {
      if (!this.authService.isLoggedIn()) {
        this.router.navigate(['/login']);
      }
    }
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    const formValue = this.form.value;
    if (formValue.newPassword !== formValue.confirmPassword) {
      console.log('Mật khẩu mới và xác nhận mật khẩu không khớp');
      return;
    }

    if (formValue.logoutEverywhere) {
      this.modal.showDialog('Thông báo', 'Bạn cũng sẽ bị đăng xuất nếu như chọn Đăng xuất khỏi các thiết bị. Có muốn tiếp tục?', 'Xác nhận', true)
        .then((confirmed) => {
          if (confirmed) {
            this.authService.changePassword({
              currentPassword: formValue.currentPassword,
              newPassword: formValue.newPassword,
              logoutEverywhere: formValue.logoutEverywhere
            }).subscribe({
              next: () => {
                console.log('Đổi mật khẩu thành công');
                this.authService.logOut();
              },
              error: (err) => {
                console.log(err.message || 'Lỗi đổi mật khẩu');
              }
            });
          }
        });
    } else {
      this.authService.changePassword({
        currentPassword: formValue.currentPassword,
        newPassword: formValue.newPassword,
        logoutEverywhere: formValue.logoutEverywhere
      }).subscribe({
        next: () => {
          console.log('Đổi mật khẩu thành công');
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.log(err.message || 'Lỗi đổi mật khẩu');
        }
      });
    }
  }
}
