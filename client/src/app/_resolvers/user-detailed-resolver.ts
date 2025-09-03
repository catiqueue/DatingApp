import { ResolveFn } from '@angular/router';
import { UsersService } from '../_services/users-service';
import { inject } from '@angular/core';
import { User } from '../_models/user';

export const userDetailedResolver: ResolveFn<User | null> = (route, state) => {
  var users = inject(UsersService);
  var username = route.paramMap.get("username");
  if(!username) return null;
  return users.getUserByUsername(username);
};
