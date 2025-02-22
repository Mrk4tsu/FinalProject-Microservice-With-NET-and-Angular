import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  ElementRef,
  inject,
  OnInit,
  PLATFORM_ID,
  Renderer2,
  ViewChild
} from '@angular/core';
import {CommonModule, isPlatformBrowser} from '@angular/common';
import {User} from '../../../../auth/models/user.class';
import {Router} from '@angular/router';
import {AuthService} from '../../../../auth/services/auth.service';

@Component({
  selector: 'app-navbar-pc',
  imports: [CommonModule],
  templateUrl: './navbar-pc.component.html',
  styleUrls: [
    './navbar-pc.component.css',
    '../navbar.component.css',
    '../../../public.component.css'
  ]
})
export class NavbarPcComponent implements AfterViewInit, OnInit {
  @ViewChild('searchIcon', {static: false}) searchIcon!: ElementRef;
  @ViewChild('searchForm', {static: false}) searchForm!: ElementRef;
  @ViewChild('menu', {static: false}) menu!: ElementRef;
  user: User | null = new User();
  platForm = inject(PLATFORM_ID);
  isBrowser = isPlatformBrowser(this.platForm);

  constructor(private renderer: Renderer2,
              private cdr: ChangeDetectorRef,
              private router: Router,
              public authService: AuthService) {
  }

  ngAfterViewInit(): void {
    if (this.isBrowser) {
      if (this.searchIcon) {
        this.renderer.listen(this.searchIcon.nativeElement, 'click', () => {
          this.toggleSearch();
        });
      } else {
        console.warn('Search icon (id: search-icon) not found in the DOM.');
      }
    }
  }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platForm)) {
      this.authService.user$.subscribe(user => {
        this.user = user;
        this.cdr.detectChanges(); // Ensure the UI updates
      });
    }
  }

  onLogout(): void {
    this.authService.logOut();
  }

  toggleSearch(): void {
    if (this.searchForm && this.menu) {
      const isActive = this.searchForm.nativeElement.classList.toggle('active');

      const menuStyle = isActive ? 'none' : 'flex';
      this.renderer.setStyle(this.menu.nativeElement, 'display', menuStyle);

      // Thay đổi class của icon
      const searchIconChild = this.searchIcon.nativeElement.querySelector('i');
      if (searchIconChild) {
        const newClass = isActive ? 'fa fa-times' : 'fa fa-search';
        this.renderer.setAttribute(searchIconChild, 'class', newClass);
      }
    } else {
      console.warn('Required elements for search toggle are not found.');
    }
  }
}
