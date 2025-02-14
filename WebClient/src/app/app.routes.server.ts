import {RenderMode, ServerRoute} from '@angular/ssr';

export const serverRoutes: ServerRoute[] = [
  {
    path: '',
    renderMode: RenderMode.Client,
  },
  {
    path: 'login',
    renderMode: RenderMode.Client,
  },
  {
    path: 'register',
    renderMode: RenderMode.Client,
  },
  {
    path:'**',
    renderMode: RenderMode.Client,
  }
  // {
  //   path: 'contact',
  //   renderMode: RenderMode.Server,
  // },
  // {
  //   path: 'product/:id',
  //   renderMode: RenderMode.Prerender,
  //   async getPrerenderParams() {
  //     const posts = inject(PostService);
  //     const ids = await posts.getPosts();
  //
  //     return ids.map((id) => ({id}));
  //   },
  //   fallback: PrerenderFallback.None,
  // },
];
