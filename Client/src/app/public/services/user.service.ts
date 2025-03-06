import {inject, Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {User} from '../models/user.model';
import {map, Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  url = environment.baseUrl + 'user';
  http = inject(HttpClient);

  constructor() {
  }

  listUsername(): Observable<string[]> {
    return this.http.get<string[]>(this.url + '/list-username');
  }

  getUser(username: string): Observable<User> {
    return this.http.get<{ data: User }>(this.url + '/profile?username=' + username).pipe(
      map(response => response.data)
    );
  }
}
