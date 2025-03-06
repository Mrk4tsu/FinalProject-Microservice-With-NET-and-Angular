import {HttpErrorResponse, HttpHandlerFn, HttpInterceptorFn, HttpRequest} from '@angular/common/http';
import {AuthService} from '../auth.service';
import {catchError, switchMap, throwError} from 'rxjs';
import {inject} from '@angular/core';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  const authReq = addTokenToRequest(req, authService);
  return next(authReq).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && !req.url.includes('refresh-token')) {
        return handle401Error(authReq, next, authService);
      }
      return throwError(() => error);
    })
  );
};
const addTokenToRequest = (req: HttpRequest<unknown>, authService: AuthService): HttpRequest<unknown> => {
  const token = authService.getAccessToken();
  return token
    ? req.clone({setHeaders: {Authorization: `Bearer ${token}`}})
    : req;
};
const handle401Error = (req: HttpRequest<unknown>, next: HttpHandlerFn, authService: AuthService) => {
  return authService.refreshToken().pipe(
    switchMap(() => {
      const newRequest = addTokenToRequest(req, authService);
      return next(newRequest);
    }),
    catchError((error) => {
      authService.logOut();
      return throwError(() => error);
    })
  );
};
