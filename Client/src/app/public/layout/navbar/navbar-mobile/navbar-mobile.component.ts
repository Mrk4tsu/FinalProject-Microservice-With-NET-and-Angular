import {ChangeDetectorRef, Component, ElementRef, inject, PLATFORM_ID, Renderer2, ViewChild} from '@angular/core';
import {CommonModule, isPlatformBrowser} from '@angular/common';
import {AuthService, User} from '../../../../auth/service/auth.service';

@Component({
  selector: 'app-navbar-mobile',
  imports: [CommonModule],
  templateUrl: './navbar-mobile.component.html',
  styleUrls: [
    './navbar-mobile.component.css',
    '../navbar.component.css',
    '../../public/public.component.css']
})
export class NavbarMobileComponent {
  @ViewChild('menuToggle', {static: false}) menuToggle!: ElementRef;
  @ViewChild('sidebar', {static: false}) sidebar!: ElementRef;
  @ViewChild('overlay', {static: false}) overlay!: ElementRef;
  @ViewChild('closeSidebar', {static: false}) closeSidebar!: ElementRef;
  platform = inject(PLATFORM_ID);
  user: User | null = new User();
  isBrowser = isPlatformBrowser(this.platform);

  constructor(private renderer: Renderer2,
              public authService: AuthService,
              private cdr: ChangeDetectorRef) {
  }

  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.cdr.detectChanges(); // Ensure the UI updates
    });
  }

  ngAfterViewInit(): void {
    if (this.menuToggle && this.sidebar && this.overlay) {
      this.renderer.listen(this.menuToggle.nativeElement, 'click', () => {
        this.toggleSidebar(true);
      });
      this.renderer.listen(this.overlay.nativeElement, 'click', () => {
        this.toggleSidebar(false);
      });
      if (this.closeSidebar) {
        this.renderer.listen(this.closeSidebar.nativeElement, 'click', () => {
          this.toggleSidebar(false);
        });
      }
    } else {
      console.warn('Menu toggle, sidebar, or overlay elements are not found in the DOM.');
    }
  }

  onLogout(): void {
    
  }

  toggleSidebar(isActive: boolean): void {
    if (this.sidebar && this.overlay) {
      if (isActive) {
        this.renderer.addClass(this.sidebar.nativeElement, 'active');
        this.renderer.addClass(this.overlay.nativeElement, 'active');
      } else {
        this.renderer.removeClass(this.sidebar.nativeElement, 'active');
        this.renderer.removeClass(this.overlay.nativeElement, 'active');
      }
    }
  }
}
