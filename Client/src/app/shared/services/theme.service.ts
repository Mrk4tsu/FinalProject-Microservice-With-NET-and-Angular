import {inject, Injectable, PLATFORM_ID} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {isPlatformBrowser} from '@angular/common';
import {CookieService} from 'ngx-cookie-service';
import {THEME} from '../constant';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private themeSubject = new BehaviorSubject<string>('light-theme');
  theme$ = this.themeSubject.asObservable();
  platForm = inject(PLATFORM_ID);

  constructor(private cookieService: CookieService) {
    if (isPlatformBrowser(this.platForm)) {
      this.load()
    }
  }

  load() {
    const savedTheme = this.cookieService.get(THEME) || 'light-theme';
    this.setTheme(savedTheme);
  }
  setTheme(theme: string) {
    const htmlElement = document.documentElement;
    htmlElement.classList.remove('light-theme', 'dark-theme');
    htmlElement.classList.add(theme);
    this.cookieService.set(THEME, theme);
    this.themeSubject.next(theme);
  }
  getCurrentTheme(): string {
    return this.themeSubject.value
  }
}
