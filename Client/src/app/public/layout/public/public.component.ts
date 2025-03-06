import {ChangeDetectionStrategy, ChangeDetectorRef, Component} from '@angular/core';
import {NavbarComponent} from '../navbar/navbar.component';
import {RouterOutlet} from '@angular/router';
import {FooterComponent} from '../footer/footer.component';
import {LoginModalComponent} from '../../../auth/pages/login-modal/login-modal.component';
import {ThemeService} from '../../../shared/services/theme.service';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-public',
  imports: [
    NavbarComponent,
    RouterOutlet,
    FooterComponent,
    LoginModalComponent,
    CommonModule
  ],
  templateUrl: './public.component.html',
  styleUrl: './public.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PublicComponent {
  isDarkTheme = false;

  constructor(private cdr: ChangeDetectorRef, public themeService: ThemeService) {
    this.isDarkTheme = this.themeService.getCurrentTheme() === 'dark-theme';
    this.themeService.theme$.subscribe((theme) => {
      this.isDarkTheme = theme === 'dark-theme';
    });
  }

  ngOnInit(): void {
    this.themeService.theme$.subscribe(theme => {
      document.body.className = theme;
      this.cdr.detectChanges();
    });
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
