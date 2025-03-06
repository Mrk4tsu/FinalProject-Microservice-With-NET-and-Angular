import {Component, ElementRef, Renderer2} from '@angular/core';

@Component({
  selector: 'app-topbar',
  imports: [],
  templateUrl: './topbar.component.html',
  styleUrls: [
    './topbar.component.css',
    '../admin/admin.component.css'
  ]
})
export class TopbarComponent {
  constructor(
    private renderer: Renderer2,
    private el: ElementRef,
  ) {
  }

  ngOnInit(): void {
    const sidebarToggleTop = this.el.nativeElement.querySelector('#sidebarToggleTop');
    if (sidebarToggleTop) {
      this.renderer.listen(sidebarToggleTop, 'click', () => this.toggleSidebar());
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
}
