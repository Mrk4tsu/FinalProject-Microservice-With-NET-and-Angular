import {Component, inject} from '@angular/core';
import {ModalService} from '../../../../shared/services/modal.service';
import Swiper from 'swiper';
import {Autoplay, Navigation, Pagination} from 'swiper/modules';

@Component({
  selector: 'app-recommend',
  imports: [],
  templateUrl: './recommend.component.html',
  styleUrl: './recommend.component.css'
})
export class RecommendComponent {
  modal = inject(ModalService);
  ngOnInit() {
      this.initSwiper();
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

  isDevelopment() {
    this.modal.showDialogInfo('Chức năng đang được phát triển');
  }
}
