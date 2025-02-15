
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
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
    'index.csr.html': {size: 6777, hash: '4ec8c5ae307907af8fa00e7e617ecb8fe544952fffdd74f779df062d5d28b228', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1498, hash: '83388687bcd3dec5f1ff750dc9575eb98e24e0b1251d461d1484e7b9b0e7f12b', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'styles-MRQYNWVT.css': {size: 260918, hash: '/cvqyT/eGbU', text: () => import('./assets-chunks/styles-MRQYNWVT_css.mjs').then(m => m.default)}
  },
};
