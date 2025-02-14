import {inject, Injectable, PLATFORM_ID} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ModalComponent} from '../components/modal/modal.component';
import {isPlatformBrowser} from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  platForm = inject(PLATFORM_ID);

  constructor(private modalService: NgbModal) {
    if (isPlatformBrowser(this.platForm)) {
    }
  }

  showDialog(title: string, message: string, btnText: string = 'OK', cancelBtn: boolean): Promise<boolean> {
    const modalRef = this.modalService.open(ModalComponent);
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.btnText = btnText;
    modalRef.componentInstance.showCancel = cancelBtn;
    return modalRef.result.then(() => true).catch(() => false);
  }
}
