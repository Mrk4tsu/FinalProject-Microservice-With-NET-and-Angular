import {AfterViewInit, ChangeDetectorRef, Component, ElementRef, OnInit, Renderer2, ViewChild} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {ProductService} from '../../../services/product.service';
import {ModalService} from '../../../../shared/services/modal.service';
import {AuthService} from '../../../../auth/services/auth.service';
import {User} from '../../../../auth/models/user.class';
import {CommonModule} from '@angular/common';

@Component({
  selector: 'app-navbar-pc',
  imports: [CommonModule],
  templateUrl: './navbar-pc.component.html',
  styleUrls: [
    './navbar-pc.component.css',
    '../navbar.component.css',
    '../../public/public.component.css'
  ]
})
export class NavbarPcComponent implements AfterViewInit, OnInit {
  @ViewChild('searchIcon', {static: false}) searchIcon!: ElementRef;
  @ViewChild('searchForm', {static: false}) searchForm!: ElementRef;
  @ViewChild('menu', {static: false}) menu!: ElementRef;
  user: User | null = new User();
  categories$ = this.productService.categories$;

  constructor(private renderer: Renderer2,
              private cdr: ChangeDetectorRef,
              private route: ActivatedRoute,
              public productService: ProductService,
              private modal: ModalService,
              public authService: AuthService) {
  }

  ngAfterViewInit(): void {
    if (this.searchIcon) {
      this.renderer.listen(this.searchIcon.nativeElement, 'click', () => {
        this.toggleSearch();
      });
    } else {
      console.warn('Search icon (id: search-icon) not found in the DOM.');
    }
  }

  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.cdr.detectChanges(); // Ensure the UI updates
    });
    this.productService.loadCategories();
  }

  onLogout(): void {
    this.modal.showDialog('Thông báo', 'Bạn có chắc chắn muốn đăng xuất?', 'Đăng xuất', true).then(result => {
      if (result) {
        this.authService.logOut();
      }
    });
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

  isDevelopment() {
    this.modal.showDialogInfo('Chức năng đang được phát triển');
  }
}
