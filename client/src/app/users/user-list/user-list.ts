import { Component, inject, OnInit } from '@angular/core';
import { UsersService } from '../../_services/users-service';
import { User } from '../../_models/user';
import { UserCard } from "../user-card/user-card";

@Component({
  selector: 'app-user-list',
  imports: [UserCard],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList implements OnInit {
  private usersService = inject(UsersService);
  users: User[] = [];

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers() {
    this.usersService.getUsers().subscribe({
      next: users => this.users = users
    });
  }
}
