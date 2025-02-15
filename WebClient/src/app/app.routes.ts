import {Routes} from '@angular/router';
import {ContactComponent} from './contact/contact.component';
import {ProductComponent} from './product/product.component';
import {HomeComponent} from './public/pages/home/home.component';
import {AuthComponent} from './auth/auth.component';
import {LoginComponent} from './auth/pages/login/login.component';
import {TestComponent} from './test/test/test.component';

export const routes: Routes = [
  {
    path: '',
    children: [
      {path: '', component: HomeComponent}
    ]
  },
  {
    path: '',
    component: AuthComponent,
    children: [
      {path: 'login', component: LoginComponent},
    ]
  },
  {
    path: '',
    component: TestComponent,
    loadChildren: () => import('./test/test.module').then(m => m.TestModule)
  },
  {path: 'contact', component: ContactComponent},
  {path: 'product/:id', component: ProductComponent},
];
