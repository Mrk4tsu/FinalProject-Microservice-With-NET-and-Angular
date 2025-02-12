import {CanActivateFn, Router} from '@angular/router';
import {inject, PLATFORM_ID} from '@angular/core';
import {AuthService} from '../auth.service';
import {isPlatformBrowser} from '@angular/common';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const platformId = inject(PLATFORM_ID);
  // Chỉ kiểm tra trên client
  if (isPlatformBrowser(platformId)) {
    if (!authService.isLoggedIn()) {
      router.navigate(['/login']);
      return false;
    }
    return true;
  }

  // Trên server cho phép điều hướng tạm thời
  return true;
};
