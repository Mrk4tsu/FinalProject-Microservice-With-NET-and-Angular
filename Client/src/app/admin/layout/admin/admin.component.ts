import {Component, ElementRef, HostListener, Renderer2} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {TopbarComponent} from '../topbar/topbar.component';
import {SidebarComponent} from '../sidebar/sidebar.component';

@Component({
  selector: 'app-admin',
  imports: [
    RouterOutlet,
    TopbarComponent,
    SidebarComponent
  ],
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent {
  constructor(
    private renderer: Renderer2,
    private el: ElementRef,
  ) {
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
