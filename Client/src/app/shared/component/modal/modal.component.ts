import {Component, Input} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-modal',
  imports: [],
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.css'
})
export class ModalComponent {
  @Input() title!: string;
  @Input() message!: string;
  @Input() btnText: string = 'OK';
  @Input() showCancel = false;
  constructor(public activeModal: NgbActiveModal) {}
}
