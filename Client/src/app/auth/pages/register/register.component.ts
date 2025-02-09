import {Component} from '@angular/core';
import {AbstractControl, FormBuilder, FormsModule, ReactiveFormsModule, ValidatorFn, Validators} from '@angular/forms';
import {FirstkeyPipe} from '../../service/helper/firstkey.pipe';
import {AuthService} from '../../service/auth.service';
import {ToastrService} from 'ngx-toastr';
import {catchError, throwError} from 'rxjs';
@Component({
  selector: 'app-register',
  imports: [FormsModule, ReactiveFormsModule, FirstkeyPipe],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  isSubmitted: boolean = false;
  isLoading: boolean = false;

  constructor(public formBuilder: FormBuilder, private service: AuthService, private toast: ToastrService) {
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
  }, { validators: this.passwordMatchValidator() });

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
            this.isLoading = false;
            this.toast.success('Account created successfully', 'Success');
          }
        }
      });
    }
  }
}
