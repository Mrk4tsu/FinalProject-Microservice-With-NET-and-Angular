import {isPlatformBrowser} from '@angular/common';
import {inject, Injectable, PLATFORM_ID} from '@angular/core';
import {jwtDecode} from 'jwt-decode';
import {User} from '../models/user.class';
import {BehaviorSubject, catchError, filter, Observable, switchMap, take, tap, throwError} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';
import {CookieService} from 'ngx-cookie-service';
import {environment} from '../../../environments/environment';
import {ACCESS_TOKEN_KEY, CLIENT_ID_KEY, REFRESH_TOKEN_KEY} from '../../shared/helper/constant';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private userSubject = new BehaviorSubject<User | null>(null);
  user$: Observable<User | null> = this.userSubject.asObservable();
  private refreshTokenSubject = new BehaviorSubject<any>(null);
  flatForm = inject(PLATFORM_ID);

  constructor(private http: HttpClient,
              private router: Router,
              private cookieService: CookieService) {
    this.loadUserFromToken();
  }

  urlAuth = environment.baseUrl + 'auth';
  urlUser = environment.baseUrl + 'user';

  confirmPassword(username: string, token: string, formValue: any) {
    const encodedToken = encodeURIComponent(token);
    return this.http.post(this.urlUser + '/reset-password', {username, token: encodedToken, ...formValue});
  }

  requestForgotPassword(formData: any) {
    return this.http.post(this.urlUser + '/request-forgot', {...formData});
  }

  register(formData: any) {
    return this.http.post(this.urlAuth + '/register', formData);
  }

  login(formData: any) {
    const userAgent = isPlatformBrowser(this.flatForm)
      ? navigator.userAgent
      : 'Server';
    const clientId = this.getClientId();
    return this.http.post(this.urlAuth + '/login', {...formData, clientId, userAgent}).pipe(
      tap((res: any) => {
        const {accessToken, refreshToken, clientId, refreshTokenExpiry} = res.data;
        this.saveToken(accessToken, refreshToken, clientId, refreshTokenExpiry);

        this.userSubject.next(this.decodeTokenToUser(accessToken));
      })
    );
  }

  refreshToken(): Observable<any> {
    if (this.refreshTokenSubject.value === null) {
      this.refreshTokenSubject.next(null);
      const refreshToken = this.getRefreshToken();
      const clientId = this.getClientId();

      if (!refreshToken || !clientId) {
        this.deleteToken();
        return throwError(() => new Error('Missing credentials'));
      }
      const userId = this.decodeTokenToUser(this.getAccessToken())?.userId;
      return this.http.post<any>(this.urlAuth + '/refresh-token', {
        refreshToken,
        clientId,
        userId
      }).pipe(
        tap((res) => {
          const {accessToken, refreshToken, clientId, refreshTokenExpiry} = res.data;
          this.saveToken(accessToken, refreshToken, clientId, refreshTokenExpiry);
          this.refreshTokenSubject.next(accessToken);
        }),
        catchError((error) => {
          this.refreshTokenSubject.next('error');
          this.deleteToken();
          return throwError(() => error);
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap((): Observable<any> => {
          console.log('Token refreshed');
          return this.refreshToken();
        })
      );
    }
  }

  logOut() {
    const currentUrl = this.router.url;
    const userId = this.getCurrentUser()?.userId;
    const clientId = this.getClientId();
    this.http.post(this.urlAuth + '/logout', {userId, clientId}).subscribe({
      next: () => {
        this.deleteToken();
        this.cookieService.delete(CLIENT_ID_KEY, '/');

        this.updateUser(null);
        this.router.navigateByUrl(currentUrl).then(() => {
          this.reloadWindow();
        });
      },
      error: (err) => {
        this.deleteToken();
        this.cookieService.delete(CLIENT_ID_KEY, '/');
        this.updateUser(null);

        this.router.navigateByUrl(currentUrl).then(() => {
          this.reloadWindow();
        });
      }
    });
  }

  saveToken(accessToken: string, refreshToken: string, clientId: string, refreshTokenExpiry: string) {
    const expiryDate = new Date(refreshTokenExpiry);

    this.cookieService.set(ACCESS_TOKEN_KEY, accessToken, expiryDate, '/', '', true, 'Strict');
    this.cookieService.set(REFRESH_TOKEN_KEY, refreshToken, expiryDate, '/', '', true, 'Strict');
    this.cookieService.set(CLIENT_ID_KEY, clientId, expiryDate, '/', '', true, 'Strict');
  }

  deleteToken() {
    this.cookieService.delete(ACCESS_TOKEN_KEY, '/');
    this.cookieService.delete(REFRESH_TOKEN_KEY, '/');
    this.userSubject.next(null);
  }

  isLoggedIn() {
    return (this.getAccessToken() != null && this.getCurrentUser() != null);
  }

  getCurrentUser(): User | null {
    return this.userSubject.value;
  }

  getAccessToken() {
    return this.cookieService.get(ACCESS_TOKEN_KEY);
  }

  getRefreshToken() {
    return this.cookieService.get(REFRESH_TOKEN_KEY);
  }

  getClientId(): string {
    return this.cookieService.get(CLIENT_ID_KEY);
  }

  updateUser(user: User | null) {
    this.userSubject.next(user);
  }

  private loadUserFromToken(): void {
    const token = this.getAccessToken();
    if (token) {
      const user = this.decodeTokenToUser(token);
      if (user) {
        this.userSubject.next(user);
        console.log('User loaded from token', user.userId);
      }
    }
  }

  decodeTokenToUser(token: string): User | null {
    try {
      const decoded: any = jwtDecode(token);
      return {
        userId: decoded.UserId || decoded.userId,
        fullName: decoded.FullName || decoded.fullName,
        avatar: decoded.Avatar || decoded.avatar,
        role: decoded.Role || decoded.role,
      };
    } catch {
      return null;
    }
  }

  reloadWindow() {
    if (isPlatformBrowser(this.flatForm))
      window.location.reload();
  }
}
