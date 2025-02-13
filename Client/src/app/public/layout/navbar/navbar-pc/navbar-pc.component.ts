import {Component, ElementRef, inject, PLATFORM_ID, Renderer2, ViewChild} from '@angular/core';
import {isPlatformBrowser} from '@angular/common';

@Component({
  selector: 'app-navbar-pc',
  imports: [],
  templateUrl: './navbar-pc.component.html',
  styleUrls: [
    './navbar-pc.component.css',
    '../navbar.component.css',
    '../../public/public.component.css'
  ]
})
export class NavbarPcComponent {
  @ViewChild('searchIcon', {static: false}) searchIcon!: ElementRef;
  @ViewChild('searchForm', {static: false}) searchForm!: ElementRef;
  @ViewChild('menu', {static: false}) menu!: ElementRef;
  platform = inject(PLATFORM_ID);
  isBrowser = isPlatformBrowser(this.platform);

  constructor(private renderer: Renderer2) {
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
  toggleSearch(): void {
    if (this.searchForm && this.menu) {
      const isActive = this.searchForm.nativeElement.classList.toggle('active');

      // Thay đổi CSS của menu
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
