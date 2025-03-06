import {Injectable} from '@angular/core';
import {ModalComponent} from '../components/modal/modal.component';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

@Injectable({
  providedIn: 'root'
})
export class ModalService {

  constructor(private modalService: NgbModal) {
  }

  showDialog(title: string, message: string, btnText: string = 'Đồng ý', cancelBtn: boolean): Promise<boolean> {
    const modalRef = this.modalService.open(ModalComponent);
    modalRef.componentInstance.title = title;
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.btnText = btnText;
    modalRef.componentInstance.showCancel = cancelBtn;
    return modalRef.result.then(() => true).catch(() => false);
  }

  showDialogInfo(message: string) {
    const modalRef = this.modalService.open(ModalComponent);
    modalRef.componentInstance.title = "Thông báo";
    modalRef.componentInstance.message = message;
    modalRef.componentInstance.btnText = "Đồng ý";
    modalRef.componentInstance.btnStyle = "bg-info text-white";
    modalRef.componentInstance.showCancel = false;
  }
}
