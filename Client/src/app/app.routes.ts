import {Routes} from '@angular/router';
import {CropImageComponent} from './shared/component/crop-image/crop-image.component';

export const routes: Routes = [
  {
    path: '', loadChildren: () => import('./public/public.module').then(m => m.PublicModule)
  },
  {
    path: '', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'avatar', component: CropImageComponent
  },
  { path: '', redirectTo: '/', pathMatch: 'full' }
];
