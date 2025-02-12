import {Component, OnInit} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {AuthService} from './auth/service/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Client';

  constructor(private auth: AuthService) {
  }

  ngOnInit() {
  }
}
