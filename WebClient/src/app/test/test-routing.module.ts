import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {TestComponent} from './test/test.component';
import {TestPageComponent} from './pages/test-page/test-page.component';

const routes: Routes = [
  {
    path: '',
    component: TestComponent,
    children: [
      {path: 'test', component: TestPageComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TestRoutingModule {
}
