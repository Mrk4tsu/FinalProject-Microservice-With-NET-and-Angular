import {ChangeDetectorRef, Component} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {AuthService, User} from '../../service/auth.service';

@Component({
  selector: 'app-auth',
  imports: [
    RouterOutlet
  ],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.css'
})
export class AuthComponent {
  user: User | null = new User();
  constructor(public authService: AuthService, private cdr: ChangeDetectorRef) {}
  ngOnInit(): void {
    this.authService.user$.subscribe(user => {
      this.user = user;
      this.cdr.detectChanges(); // Ensure the UI updates
    });
  }
}
