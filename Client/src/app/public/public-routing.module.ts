import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {PublicComponent} from './layout/public/public.component';
import {HomeComponent} from './pages/home/home.component';
import {UserComponent} from './pages/user/user.component';
import {ProfileComponent} from './pages/user/profile/profile.component';
import {ProfileResolver} from './services/helper/user.resolver';

const routes: Routes = [
  {
    path: '',
    component: PublicComponent,
    children: [
      {path: '', component: HomeComponent},
      {
        path: 'profile',
        component: UserComponent,
        children: [
          {
            path: ':username',
            component: ProfileComponent,
            resolve: {userData: ProfileResolver}
          }
        ],
      }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule {
}
