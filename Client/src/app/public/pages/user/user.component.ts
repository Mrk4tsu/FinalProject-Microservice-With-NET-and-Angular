import {AfterViewInit, Component, OnInit} from '@angular/core';
import {ModalService} from '../../../shared/component/modal/modal.service';
import {ModalComponent} from '../../../shared/component/modal/modal.component';
import {AuthService} from '../../../auth/service/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-user',
  imports: [],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements AfterViewInit {
  constructor(private modalService: ModalService,
              private router: Router,
              private auth: AuthService) {

  }

  ngAfterViewInit() {
    if (!this.auth.isLoggedIn()) {
      this.modalService.showLoginRequired()
        .then((confirmed) => {
          if (confirmed) {
            this.router.navigate(['/login']);
          }
        });
    }
  }
}
