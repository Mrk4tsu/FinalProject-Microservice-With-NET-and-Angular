import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PostService {
  baseUrl = 'https://jsonplaceholder.typicode.com';
  getPosts() {
    return Promise.resolve(['1', '2', '3']);
  }
}
