import {CanActivateFn, Router} from '@angular/router';
import {inject,} from '@angular/core';
import {AuthService} from '../auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (!authService.isLoggedIn()) {
    router.navigate(['/login']).then();
    return false;
  }
  return true;
};
