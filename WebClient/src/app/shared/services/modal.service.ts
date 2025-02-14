import {Injectable} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ModalComponent} from '../components/modal/modal.component';

@Injectable({
  providedIn: 'root'
})
export class ModalService {

  constructor(private modalService: NgbModal) {
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
