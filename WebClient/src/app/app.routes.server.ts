import {RenderMode, ServerRoute} from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: '',
    renderMode: RenderMode.Server,
  },
  {
    path: 'login',
    renderMode: RenderMode.Server,
  },
  {
    path: 'register',
    renderMode: RenderMode.Server,
  },
  {
    path:'**',
    renderMode: RenderMode.Client,
  }
];
