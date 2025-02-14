import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {CookieService} from 'ngx-cookie-service';
import {THEME} from '../constant';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private themeSubject = new BehaviorSubject<string>('light-theme');
  theme$ = this.themeSubject.asObservable();

  constructor(private cookieService: CookieService) {
    this.load()
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
