import {AfterViewInit, Component, inject, OnDestroy, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {map, Observable} from 'rxjs';
import {User} from '../../../models/user.model';
import {AuthService} from '../../../../auth/services/auth.service';
import {Fancybox} from '@fancyapps/ui';
import {CommonModule} from '@angular/common';
import {SeoService} from '../../../../shared/services/seo.service';

@Component({
  selector: 'app-profile',
  imports: [CommonModule],
  templateUrl: './profile.component.html',
  styleUrls: [
    './profile.component.css',
    '../../../layout/public/public.component.css'
  ]
})
export class ProfileComponent implements OnInit, AfterViewInit, OnDestroy {
  private route = inject(ActivatedRoute);
  private fancyboxInitialized = false;
  user$: Observable<User | null> | null = null;
  isMe = false;

  constructor(public authService: AuthService, private seo: SeoService) {
  }

  ngOnInit(): void {
    const userData = this.route.snapshot.data['userData'];
    if (userData) {
      this.seo.updateSeo(
        `${userData.fullName} - Profile`,
        userData.fullName,
        userData.avatar
      );
      if (userData.userName === this.authService.getCurrentUser()?.username) {
        this.isMe = true;
      }
    }

    // Giữ observable cho template (nếu cần)
    this.user$ = this.route.data.pipe(map((data: any) => data.userData));
  }

  ngAfterViewInit(): void {
    this.initializeFancybox();
  }

  private initializeFancybox(): void {
    this.user$?.subscribe(() => {
      setTimeout(() => {
        if (this.fancyboxInitialized) {
          Fancybox.unbind("[data-fancybox]");
          Fancybox.unbind("[data-caption]");
          Fancybox.destroy();
        }

        Fancybox.bind("[data-fancybox]");
        Fancybox.bind("[data-caption]");
        this.fancyboxInitialized = true;
      });
    });
  }

  ngOnDestroy(): void {
    if (this.fancyboxInitialized) {
      Fancybox.destroy();
    }
  }
}
