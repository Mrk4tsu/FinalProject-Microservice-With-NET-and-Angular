import { Component } from '@angular/core';
import {NavbarPcComponent} from './navbar-pc/navbar-pc.component';
import {NavbarMobileComponent} from './navbar-mobile/navbar-mobile.component';

@Component({
  selector: 'app-navbar',
  imports: [
    NavbarPcComponent,
    NavbarMobileComponent
  ],
  templateUrl: './navbar.component.html',
  styleUrls: [
    './navbar.component.css',
    '../public/public.component.css'
  ]
})
export class NavbarComponent {

}
