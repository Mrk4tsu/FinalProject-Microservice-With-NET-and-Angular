import { Component } from '@angular/core';
import {NavbarMobileComponent} from './navbar-mobile/navbar-mobile.component';
import {NavbarPcComponent} from './navbar-pc/navbar-pc.component';

@Component({
  selector: 'app-navbar',
  imports: [
    NavbarMobileComponent,
    NavbarPcComponent
  ],
  templateUrl: './navbar.component.html',
  styleUrls: [
    './navbar.component.css',
    '../../public.component.css'
  ]
})
export class NavbarComponent {

}
