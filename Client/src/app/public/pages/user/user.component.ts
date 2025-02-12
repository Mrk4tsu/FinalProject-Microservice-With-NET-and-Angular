import {Component, Inject, OnInit, PLATFORM_ID} from '@angular/core';
import {AuthService} from '../../../auth/service/auth.service';
import {Router} from '@angular/router';
import {isPlatformBrowser} from '@angular/common';

@Component({
  selector: 'app-user',
  imports: [],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {
  constructor(private router: Router,
              @Inject(PLATFORM_ID) private platformId: Object,
              private auth: AuthService) {

  }

  ngOnInit() {
    if (isPlatformBrowser(this.platformId)) {
      if (!this.auth.isLoggedIn()) {
        this.router.navigate(['/login']);
      }
    }
  }
}
