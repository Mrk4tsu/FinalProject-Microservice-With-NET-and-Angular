import {inject, Injectable, makeStateKey, PLATFORM_ID, TransferState} from '@angular/core';
import {Meta, Title} from '@angular/platform-browser';
import {WEB_DESCRIPTION, WEB_IMAGE, WEB_NAME} from '../helper/constant';
import {Router} from '@angular/router';
import {environment} from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SeoService {
  domain = environment.baseDomain;
  private transferState = inject(TransferState);

  constructor(private title: Title, private meta: Meta, private router: Router) {
  }

// seo.service.ts
  updateSeo(title: string | null, description: string | null, image: string | null) {
    const url = this.router.url;
    const finalTitle = title ? `${title}${WEB_NAME}` : WEB_NAME;
    const finalDescription = description || WEB_DESCRIPTION;
    const finalImage = image || WEB_IMAGE;

    this.title.setTitle(finalTitle);
    this.meta.updateTag({name: 'description', content: finalDescription});

    // Cập nhật tất cả meta tags với giá trị đã xử lý
    this.meta.updateTag({property: 'og:title', content: finalTitle});
    this.meta.updateTag({property: 'og:description', content: finalDescription});
    this.meta.updateTag({property: 'og:image', content: finalImage});
    this.meta.updateTag({property: 'og:url', content: this.domain + url});

    this.meta.updateTag({property: 'twitter:title', content: finalTitle});
    this.meta.updateTag({property: 'twitter:description', content: finalDescription});
    this.meta.updateTag({property: 'twitter:image', content: finalImage});
  }
}
