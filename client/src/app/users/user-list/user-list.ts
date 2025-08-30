import { Component, inject, OnInit } from '@angular/core';
import { UsersService } from '../../_services/users-service';
import { UserCard } from "../user-card/user-card";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { UsersCacheService } from '../../_services/cache/users-cache';

@Component({
  selector: 'app-user-list',
  imports: [UserCard, PaginationModule, FormsModule, ButtonsModule],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList implements OnInit {
  usersService = inject(UsersService);
  usersCache = inject(UsersCacheService);

  ngOnInit(): void {
    if(!this.usersCache.users()?.length) this.usersService.resetCache();
  }

  pageChanged(event: any) {
    var pagination = this.usersCache.pagination();
    if(!pagination) return;
    if(pagination.current.pageNumber === event.page) return;
    pagination.current.pageNumber = event.page;
    this.usersCache.pagination.set(pagination);
    this.usersService.loadUsers();
  }
}
