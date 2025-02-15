import {ChangeDetectorRef, Component, inject, OnInit, PLATFORM_ID} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {ThemeService} from '../shared/services/theme.service';
import {CommonModule, isPlatformBrowser} from '@angular/common';
import {NavbarComponent} from './layout/navbar/navbar.component';
import {FooterComponent} from './layout/footer/footer.component';

@Component({
  selector: 'app-public',
  imports: [
    RouterOutlet,
    NavbarComponent,
    FooterComponent,
    CommonModule
  ],
  templateUrl: './public.component.html',
  styleUrl: './public.component.css'
})
export class PublicComponent implements OnInit {
  isDarkTheme = false;
  platForm = inject(PLATFORM_ID);

  constructor(private cdr: ChangeDetectorRef, public themeService: ThemeService) {
    if (isPlatformBrowser(this.platForm)) {
      this.isDarkTheme = this.themeService.getCurrentTheme() === 'dark-theme';
      this.themeService.theme$.subscribe((theme) => {
        this.isDarkTheme = theme === 'dark-theme';
      });
    }
  }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platForm)) {
      this.themeService.theme$.subscribe(theme => {
        document.body.className = theme;
        this.cdr.detectChanges();
      });
    }
  }

  changeTheme() {
    const newTheme = this.themeService.getCurrentTheme() === 'light-theme'
      ? 'dark-theme'
      : 'light-theme';
    this.themeService.setTheme(newTheme);
    try {
      sessionStorage.setItem('THEME', newTheme); // Backup cho SSR
    } catch (e) {
    }
  }
}
