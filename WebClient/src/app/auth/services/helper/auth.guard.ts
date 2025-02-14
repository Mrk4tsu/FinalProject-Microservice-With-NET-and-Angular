import {CanActivateFn} from '@angular/router';
import {inject, PLATFORM_ID} from '@angular/core';
import {AuthService} from '../auth.service';
import {Router} from 'express';
import {isPlatformBrowser} from '@angular/common';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);
    if (!authService.isLoggedIn()) {
      router.navigate(['/login']);
      return false;
    }
    return true;
};
