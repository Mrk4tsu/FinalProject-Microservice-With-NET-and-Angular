import {Component, inject} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Component({
  selector: 'app-home',
  imports: [],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  http = inject(HttpClient)

  ngOnInit() {
    this.http.get('https://jsonplaceholder.typicode.com/posts').subscribe(data => {
      console.log(data);
    });
  }
}
