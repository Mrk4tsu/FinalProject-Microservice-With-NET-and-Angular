import {Component, inject, OnInit, PLATFORM_ID, Renderer2} from '@angular/core';
import {CommonModule, isPlatformBrowser} from '@angular/common';
import {RecommendComponent} from './recommend/recommend.component';
import {TopBlogComponent} from './top-blog/top-blog.component';

@Component({
  selector: 'app-home',
  imports: [
    CommonModule,
    RecommendComponent,
    TopBlogComponent
  ],
  templateUrl: './home.component.html',
  styleUrls: [
    './home.component.css',
    '../../layout/public/public.component.css'
  ]
})
export class HomeComponent implements OnInit {
  platForm = inject(PLATFORM_ID);

  constructor(private renderer: Renderer2) {
  }

  ngOnInit(): void {
    if (isPlatformBrowser(this.platForm)) {
      this.initializeSeasonalThemes();
    }
  }

  initializeSeasonalThemes(): void {
    const today = new Date();
    const startChristmas = new Date(today.getFullYear(), 11, 1);
    const endChristmas = new Date(today.getFullYear(), 11, 30);

    const startLunar = new Date(today.getFullYear(), 0, 1);
    const endLunar = new Date(today.getFullYear(), 1, 28);

    const imageSection = document.querySelector('.image-section');
    if (imageSection) {
      if (today >= startChristmas && today <= endChristmas) {
        this.renderer.addClass(imageSection, 'christmas');
      } else if (today >= startLunar && today <= endLunar) {
        this.renderer.addClass(imageSection, 'lunar');
      }
    }
  }
}
