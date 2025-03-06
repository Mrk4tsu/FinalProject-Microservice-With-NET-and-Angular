import {ChangeDetectorRef, Component, NgZone} from '@angular/core';
import {Subscription, timer} from 'rxjs';

@Component({
  selector: 'app-banner',
  imports: [],
  templateUrl: './banner.component.html',
  styleUrl: './banner.component.css'
})
export class BannerComponent {
  eventDate = new Date('2025-05-31T23:59:59').getTime();
  days: number = 0;
  hours: number = 0;
  minutes: number = 0;
  seconds: number = 0;
  private countdownSub!: Subscription;

  constructor(
    private ngZone: NgZone,
    private cdr: ChangeDetectorRef
  ) {
  }

  ngOnInit(): void {
    this.ngZone.runOutsideAngular(() => {
      // Khởi tạo timer bên ngoài Angular Zone để tối ưu hiệu suất
      this.countdownSub = timer(0, 1000).subscribe(() => {
        this.ngZone.run(() => { // Quay lại Angular Zone để cập nhật UI
          this.updateCountdown();
          this.cdr.detectChanges(); // Force change detection
        });
      });
    });
  }

  ngOnDestroy(): void {
    if (this.countdownSub) {
      this.countdownSub.unsubscribe();
    }
  }

  private updateCountdown(): void {
    const now = Date.now(); // Sử dụng client-side time
    const timeLeft = this.eventDate - now;

    if (timeLeft > 0) {
      this.days = Math.floor(timeLeft / (1000 * 60 * 60 * 24));
      this.hours = Math.floor((timeLeft % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      this.minutes = Math.floor((timeLeft % (1000 * 60 * 60)) / (1000 * 60));
      this.seconds = Math.floor((timeLeft % (1000 * 60)) / 1000);
    } else {
      this.resetTimer();
    }
  }

  private resetTimer(): void {
    this.days = this.hours = this.minutes = this.seconds = 0;
    this.countdownSub?.unsubscribe();
  }
}
