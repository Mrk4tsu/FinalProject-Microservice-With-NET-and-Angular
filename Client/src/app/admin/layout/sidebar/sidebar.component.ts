import {Component, ElementRef, HostListener, Renderer2} from '@angular/core';

@Component({
  selector: 'app-sidebar',
  imports: [],
  templateUrl: './sidebar.component.html',
  styleUrls: [
    './sidebar.component.css',
    '../admin/admin.component.css'
  ]
})
export class SidebarComponent {
  constructor(
    private renderer: Renderer2,
    private el: ElementRef,
  ) {
  }

  ngOnInit(): void {
    const sidebarToggle = this.el.nativeElement.querySelector('#sidebarToggle');
    if (sidebarToggle) {
      this.renderer.listen(sidebarToggle, 'click', () => this.toggleSidebar());
    }
  }

  toggleSidebar(): void {
    const body = document.body;
    const sidebar = document.querySelector('.sidebar');

    if (body.classList.contains('sidebar-toggled')) {
      this.renderer.removeClass(body, 'sidebar-toggled');
    } else {
      this.renderer.addClass(body, 'sidebar-toggled');
    }

    if (sidebar) {
      if (sidebar.classList.contains('toggled')) {
        this.renderer.removeClass(sidebar, 'toggled');
      } else {
        this.renderer.addClass(sidebar, 'toggled');
      }

      if (sidebar.classList.contains('toggled')) {
        const collapses = sidebar.querySelectorAll('.collapse');
        collapses.forEach(collapse => {
          if (collapse instanceof HTMLElement) {
            this.renderer.removeClass(collapse, 'show');
          }
        });
      }
    }
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event): void {
    const windowWidth = (event.target as Window).innerWidth;
    const sidebar = document.querySelector('.sidebar');
    if (windowWidth < 768) {
      const collapses = sidebar?.querySelectorAll('.collapse');
      collapses?.forEach(collapse => {
        if (collapse instanceof HTMLElement) {
          this.renderer.removeClass(collapse, 'show');
        }
      });
    }
    if (windowWidth < 480 && sidebar && !sidebar.classList.contains('toggled')) {
      this.renderer.addClass(document.body, 'sidebar-toggled');
      this.renderer.addClass(sidebar, 'toggled');
      const collapses = sidebar.querySelectorAll('.collapse');
      collapses.forEach(collapse => {
        if (collapse instanceof HTMLElement) {
          this.renderer.removeClass(collapse, 'show');
        }
      });
    }
  }
}
