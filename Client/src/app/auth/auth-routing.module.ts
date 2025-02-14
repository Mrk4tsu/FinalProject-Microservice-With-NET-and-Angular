import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from './pages/login/login.component';
import {RegisterComponent} from './pages/register/register.component';
import {AuthComponent} from './layout/auth/auth.component';
import {ConfirmEmailComponent} from './pages/confirm-email/confirm-email.component';
import {EmailChangeComponent} from './pages/email-change/email-change.component';
import {ForgotPasswordComponent} from './pages/forgot-password/forgot-password.component';
import {ConfirmPasswordComponent} from './pages/confirm-password/confirm-password.component';
import {ChangePasswordComponent} from './pages/change-password/change-password.component';
import {authGuard} from './service/helper/auth.guard';

const routes: Routes = [
  {
    path: '', component: AuthComponent,
    children: [
      {path: 'login', component: LoginComponent},
      {path: 'register', component: RegisterComponent},
      // {path: 'change-email', component: EmailChangeComponent, canActivate: [authGuard]},
      // {path: 'confirm-email', component: ConfirmEmailComponent, canActivate: [authGuard]},
      // {path: 'forgot-password', component: ForgotPasswordComponent, canActivate: [authGuard]},
      // {path: 'confirm-password', component: ConfirmPasswordComponent, canActivate: [authGuard]},
      // {path: 'change-password', component: ChangePasswordComponent, canActivate: [authGuard]}
    ]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule {
}
