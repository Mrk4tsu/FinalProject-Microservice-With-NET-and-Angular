import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {CookieService} from 'ngx-cookie-service';
import {ACCESS_TOKEN_KEY, CLIENT_ID_KEY, REFRESH_TOKEN_KEY} from '../../shared/constant';
import {jwtDecode} from 'jwt-decode';
import {BehaviorSubject, Observable, tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient, private cookieService: CookieService) {
    this.loadUserFromToken();
  }

  private userSubject = new BehaviorSubject<User | null>(null);
  user$: Observable<User | null> = this.userSubject.asObservable();
  url = environment.baseUrl + 'user';

  register(formData: any) {
    return this.http.post(this.url + '/register', formData);
  }
  login(formData: any) {
    const userAgent = navigator.userAgent;
    const clientId = this.getClientId();
    return this.http.post(this.url + '/login', { ...formData, clientId, userAgent }).pipe(
      tap((res: any) =>{
        const { accessToken, refreshToken, clientId, refreshTokenExpiry } = res.data;
        this.saveToken(accessToken, refreshToken, clientId, refreshTokenExpiry);

        this.userSubject.next(this.decodeTokenToUser(accessToken));
      })
    );
  }
  saveToken(accessToken: string, refreshToken: string, clientId: string, refreshTokenExpiry: string) {
    const expiryDate = new Date(refreshTokenExpiry);

    this.cookieService.set(ACCESS_TOKEN_KEY, accessToken, expiryDate, '/', '', true, 'Strict');
    this.cookieService.set(REFRESH_TOKEN_KEY, refreshToken, expiryDate, '/', '', true, 'Strict');
    this.cookieService.set(CLIENT_ID_KEY, clientId, expiryDate, '/', '', true, 'Strict');
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
