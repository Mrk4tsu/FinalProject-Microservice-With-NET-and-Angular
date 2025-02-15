import {HttpErrorResponse, HttpInterceptorFn} from '@angular/common/http';
import {inject} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import {catchError, throwError} from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastr = inject(ToastrService);
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      let errorMessage = 'An unexpected error occurred';

      switch (error.status) {
        case 400:
          errorMessage = handleBadRequestError(error);
          break;
        case 403:
          errorMessage = 'Forbidden: You do not have permission to access this resource.';
          break;
        case 404:
          errorMessage = 'Not Found: The requested resource could not be found.';
          break;
        case 500:
          errorMessage = 'Internal Server Error: Please try again later.';
          break;
        default:
          errorMessage = 'An unexpected error occurred. Please try again later.';
          break;
      }
      toastr.error(errorMessage, 'Error');
      return throwError(() => new Error(errorMessage));
    })
  );
};
const handleBadRequestError = (error: HttpErrorResponse): string => {
  if (error.error && typeof error.error === 'object') {
    const errors = error.error.errors || error.error;
    if (errors && typeof errors === 'object') {
      return Object.values(errors).join('\n');
    }
  }
  return 'Bad Request: Please check your input and try again.';
}

