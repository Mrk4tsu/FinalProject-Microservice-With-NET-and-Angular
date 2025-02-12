import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from './pages/home/home.component';
import {UserComponent} from './pages/user/user.component';
import {authGuard} from '../auth/service/helper/auth.guard';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'user', component: UserComponent, canActivate: [authGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule {
}
