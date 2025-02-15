import {PrerenderFallback, RenderMode, ServerRoute} from '@angular/ssr';
import {inject} from '@angular/core';
import {PostService} from './service/post.service';

export const serverRoutes: ServerRoute[] = [
  {
    path: '',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'test',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'login',
    renderMode: RenderMode.Prerender,
  },
  {
    path: 'contact',
    renderMode: RenderMode.Server,
  },
  {
    path: 'product/:id',
    renderMode: RenderMode.Prerender,
    async getPrerenderParams() {
      const posts = inject(PostService);
      const ids = await posts.getPosts();

      return ids.map((id) => ({id}));
    },
    fallback: PrerenderFallback.Server,
  },
];
