import {Routes} from '@angular/router';
import {PublicComponent} from './public/layout/public/public.component';
import {AuthComponent} from './auth/layout/auth/auth.component';

export const routes: Routes = [
  {
    path: '',
    component: PublicComponent,
    loadChildren: () => import('./public/public.module').then(m => m.PublicModule)
  },
  {
    path: '',
    component: AuthComponent,
    loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
  },
  { path: '**', redirectTo: '/' }
];
