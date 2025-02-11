import {inject, Injectable, PLATFORM_ID} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {CookieService} from 'ngx-cookie-service';
import {ACCESS_TOKEN_KEY, CLIENT_ID_KEY, REFRESH_TOKEN_KEY} from '../../shared/constant';
import {jwtDecode} from 'jwt-decode';
import {BehaviorSubject, catchError, filter, Observable, switchMap, take, tap, throwError} from 'rxjs';
import {isPlatformBrowser} from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  flatForm = inject(PLATFORM_ID);

  constructor(private http: HttpClient, private cookieService: CookieService) {
    this.loadUserFromToken();
  }

  private refreshTokenSubject = new BehaviorSubject<any>(null);
  private userSubject = new BehaviorSubject<User | null>(null);
  user$: Observable<User | null> = this.userSubject.asObservable();
  devices: UserDevice[] = [];
  urlAuth = environment.baseUrl + 'auth';
  urlUser = environment.baseUrl + 'user';

  requestForgotPassword(email: string) {
    return this.http.post(this.urlUser + '/forgot?email=' + email, {});
  }
  confirmPassword(token: string, newPassword: string) {
    return this.http.post(this.urlUser + '/confirm-password', {token, newPassword});
  }
  requestEmailChange(newEmail: string) {
    return this.http.post(this.urlUser + '/request?newEmail=' + newEmail, {});
  }

  confirmEmailChange(userId: string, newEmail: string, token: string) {
    const encodedToken = encodeURIComponent(token);
    return this.http.post(this.urlUser + '/confirm', {userId, newEmail, token: encodedToken});
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
          return this.refreshToken();
        })
      );
    }
    // const refreshToken = this.getRefreshToken();
    // const clientId = this.getClientId();
    // if (!refreshToken || !clientId) {
    //   console.error('Refresh token hoặc ClientID bị thiếu!');
    //   this.deleteToken();
    //   return throwError(() => new Error('Thiếu thông tin xác thực'));
    // }
    //
    // const accessToken = this.getAccessToken();
    // const userId = this.decodeTokenToUser(accessToken)?.userId;
    // return this.http
    //   .post<any>(this.url + '/refresh-token', {refreshToken, clientId, userId})
    //   .pipe(
    //     tap((res: any) => {
    //       const {accessToken, refreshToken, clientId, refreshTokenExpiry} = res.data;
    //       this.saveToken(accessToken, refreshToken, clientId, refreshTokenExpiry);
    //     }),
    //     map((res: any) => res.data.accessToken),
    //     catchError((error) => {
    //       console.error('Refresh token request failed:', error);
    //       this.deleteToken();
    //       return throwError(error);
    //     })
    //   )
  }

  logOut() {
    const userId = this.getCurrentUser()?.userId;
    const clientId = this.getClientId();
    this.http.post(this.urlAuth + '/logout', {userId, clientId}).subscribe({
      next: () => {
        this.deleteToken();
        this.cookieService.delete(CLIENT_ID_KEY, '/');
        this.updateUser(new User());
      },
      error: (err) => {
        this.deleteToken();
        this.cookieService.delete(CLIENT_ID_KEY, '/');
      }
    });
  }

  getDevices() {
    return this.http.get(this.urlAuth + '/devices');
  }

  revokeDevice(clientId: string) {
    return this.http.post(this.urlAuth + `/revoke-device?clientId=${clientId}`, {});
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

  updateUser(user: User) {
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
}

export class User {
  userId: number = 0;
  fullName: string = '';
  avatar: string = '';
  role: string = '';
}

export class UserDevice {
  clientId: string = ""
  lastLogin: string = ""
  ipAddress: string = ""
  browser: string = ""
  os: string = ""
  deviceType: string = ""
}
