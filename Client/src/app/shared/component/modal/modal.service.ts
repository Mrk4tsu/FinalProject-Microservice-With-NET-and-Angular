import {inject, Injectable, PLATFORM_ID} from '@angular/core';
import {ModalComponent} from './modal.component';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {isPlatformBrowser} from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  platformId = inject(PLATFORM_ID);
  isBrowser: boolean;

  constructor(private modalService: NgbModal) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  showLoginRequired(): Promise<boolean> {
    if (this.isBrowser) {
      const modalRef = this.modalService.open(ModalComponent);
      modalRef.componentInstance.title = 'Login Required';
      modalRef.componentInstance.message = 'Please login to continue.';
      modalRef.componentInstance.btnText = 'Đăng nhập';
      modalRef.componentInstance.showCancel = true;

      return modalRef.result.then(() => true).catch(() => false);
    }
    return new Promise<boolean>((resolve) => resolve(false));
  }

  showDialog(title: string, message: string, btnText: string = 'OK'): Promise<boolean> {
    if (this.isBrowser) {
      const modalRef = this.modalService.open(ModalComponent);
      modalRef.componentInstance.title = title;
      modalRef.componentInstance.message = message;
      modalRef.componentInstance.btnText = btnText;
      modalRef.componentInstance.showCancel = false;
      return modalRef.result.then(() => true).catch(() => false);
    }
    return new Promise<boolean>((resolve) => resolve(false));
  }
}
