import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {CreateProductComponent} from './pages/product-manage/create-product/create-product.component';
import {UpdateProductComponent} from './pages/product-manage/update-product/update-product.component';
import {ProductManageComponent} from './pages/product-manage/product-manage.component';
import {CategoryManageComponent} from './pages/category-manage/category-manage.component';
import {DashboardComponent} from './pages/dashboard/dashboard.component';
import {authGuard} from '../auth/services/helper/auth.guard';
import {AdminComponent} from './layout/admin/admin.component';

const routes: Routes = [
  {
    path: '',
    component: AdminComponent,
    canActivate: [authGuard],
    children: [
      {path: 'dashboard', component: DashboardComponent},
      {path: 'category', component: CategoryManageComponent},
      {
        path: 'product',
        component: ProductManageComponent,
        children:
          [
            {path: 'add', component: CreateProductComponent},
            {path: 'update', component: UpdateProductComponent},
          ]
      },
      {path: '', redirectTo: 'dashboard', pathMatch: 'full'},
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
}
