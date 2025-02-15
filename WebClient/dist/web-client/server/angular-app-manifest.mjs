
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: false,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 1,
    "preload": [
      "chunk-VCZYECHW.js"
    ],
    "route": "/"
  },
  {
    "renderMode": 1,
    "preload": [
      "chunk-2POTG2I5.js"
    ],
    "route": "/login"
  },
  {
    "renderMode": 1,
    "preload": [
      "chunk-2POTG2I5.js"
    ],
    "route": "/register"
  },
  {
    "renderMode": 1,
    "preload": [
      "chunk-2POTG2I5.js"
    ],
    "route": "/forgot-password"
  },
  {
    "renderMode": 1,
    "preload": [
      "chunk-2POTG2I5.js"
    ],
    "route": "/confirm-password"
  },
  {
    "renderMode": 2,
    "redirectTo": "/",
    "route": "/**"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 958, hash: 'd20e5944f3b7fc1777af02967c5c8fecef28342ce46cb9b5a40e4629ca321c21', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1498, hash: '83388687bcd3dec5f1ff750dc9575eb98e24e0b1251d461d1484e7b9b0e7f12b', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)}
  },
};
