import {ChangeDetectorRef, Component, inject, PLATFORM_ID} from '@angular/core';
import {AuthService, User} from '../../../auth/service/auth.service';
import {CommonModule, isPlatformBrowser} from '@angular/common';
import {ModalService} from '../../../shared/component/modal/modal.service';
import {Router} from '@angular/router';

declare var bootstrap: any;

@Component({
  selector: 'app-home',
  imports: [
    CommonModule
  ],
  templateUrl: './home.component.html',
  styleUrls: [
    './home.component.css',
    '../../layout/public/public.component.css'
  ]
})
export class HomeComponent {
  platForm = inject(PLATFORM_ID);

  isLoading: boolean = false;
  user: User | null = new User();
  currentDeviceId: string = '';

  constructor(public authService: AuthService,
              private modal: ModalService,
              private router: Router,
              private cdr: ChangeDetectorRef) {
  }

  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.cdr.detectChanges(); // Ensure the UI updates
    });
    if (this.authService.isLoggedIn()) {
      this.isLoading = true;
      this.authService.getDevices().subscribe({
        next: (res: any) => {
          this.isLoading = false;
          this.authService.devices = res.data;

          console.log(res);
        },
        error: (err) => {
          this.isLoading = false;
          console.error(err);
        }
      });
    }
  }

  hrefTo(url: string) {
    if (!this.authService.isLoggedIn()) {
      this.modal.showDialog('Login Required', 'Please login to continue.', 'Đăng nhập')
        .then((confirmed) => {
          if (confirmed) {
            this.router.navigate(['/login']);
          }
        });
      return;
    }
    this.router.navigate([url]);
  }

  onDeleteDevice(clientId: string) {
    if (this.isCurrentDevice(clientId)) {
      this.showConfirmLogoutModal();
    } else {
      this.deleteDevice(clientId);
    }
  }

  deleteDevice(clientId: string) {
    this.authService.revokeDevice(clientId).subscribe({
      next: (res: any) => {
        this.authService.devices = this.authService.devices.filter((device: any) => device.clientId !== clientId);
      },
      error: (err) => {
        console.error(err);
      }
    });
  }

  showConfirmLogoutModal() {
    if (isPlatformBrowser(this.platForm)) {
      const modalElement = document.getElementById('confirmLogoutModal');
      if (modalElement) {
        const modal = new bootstrap.Modal(modalElement);
        modal.show();
      }
    }
  }

  onLogout() {
    this.authService.logOut();
  }

  confirmDeleteCurrentDevice() {
    this.deleteDevice(this.currentDeviceId);
    this.onLogout();
  }

  isCurrentDevice(clientId: string): boolean {
    if (clientId === this.authService.getClientId())
      return true;
    else
      return false;
  }
}
