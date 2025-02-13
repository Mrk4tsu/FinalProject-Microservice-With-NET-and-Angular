import {Component, Inject, OnInit, PLATFORM_ID} from '@angular/core';
import {CommonModule, isPlatformBrowser} from '@angular/common';
import {register} from 'swiper/element';
import Swiper from 'swiper';
import {Autoplay, Navigation, Pagination} from 'swiper/modules';

@Component({
  selector: 'app-recommend',
  imports: [CommonModule],
  templateUrl: './recommend.component.html',
  styleUrl: './recommend.component.css',
})
export class RecommendComponent {
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
  }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platformId)) {
      register();
      this.initSwiper();
    }
  }

  initSwiper() {
    Swiper.use([Autoplay, Navigation, Pagination]);
    new Swiper('.productSwiper', {
      slidesPerView: 5,
      spaceBetween: 20,
      grabCursor: true,
      loop: true,
      autoplay: {
        delay: 3500,
        disableOnInteraction: false,
      },
      breakpoints: {
        0: {slidesPerView: 1, spaceBetween: 10},
        768: {slidesPerView: 2, spaceBetween: 15},
        1024: {slidesPerView: 4, spaceBetween: 30},
      },
    });
  }
}
