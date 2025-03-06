import {Injectable} from '@angular/core';
import {Resolve, ActivatedRouteSnapshot} from '@angular/router';
import {UserService} from '../user.service';
import {User} from '../../models/user.model';

@Injectable({providedIn: 'root'})
export class ProfileResolver implements Resolve<User | null> {
  constructor(private userService: UserService) {
  }

  resolve(route: ActivatedRouteSnapshot) {
    return this.userService.getUser(route.params['username']);
  }
}
