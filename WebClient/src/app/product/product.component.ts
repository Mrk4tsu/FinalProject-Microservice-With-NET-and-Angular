import { HttpClient } from '@angular/common/http';
import {Component, inject} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

@Component({
  selector: 'app-product',
  imports: [],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent {
  http = inject(HttpClient)
  router = inject(ActivatedRoute)
  ngOnInit() {
    this.router.params.subscribe(params => {
      console.log(params);
    })
  }
}
