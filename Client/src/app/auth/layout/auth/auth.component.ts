import {AfterViewInit, ChangeDetectorRef, Component, inject, OnInit, PLATFORM_ID} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {AuthService, User} from '../../service/auth.service';
import {ThemeService} from '../../../shared/services/theme.service';
import {isPlatformBrowser} from '@angular/common';

@Component({
  selector: 'app-auth',
  imports: [
    RouterOutlet
  ],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent implements AfterViewInit {
  platForm = inject(PLATFORM_ID);
  constructor(private themeService: ThemeService,
              private cdr: ChangeDetectorRef) {
  }

  ngAfterViewInit(): void {
    if (isPlatformBrowser(this.platForm)) {
      this.themeService.theme$.subscribe(theme => {
        document.body.className = theme;
        this.cdr.detectChanges();
      });
    }
  }
}
