import { Component, inject, OnInit } from '@angular/core';
import { UsersService } from '../../_services/users-service';
import { UserCard } from "../user-card/user-card";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { UsersCacheService } from '../../_services/cache/users-cache';
import { setPageToOne } from '../../_services/pagination-utils';
import { FilterParams } from '../../_models/filer-params';
import { AccountService } from '../../_services/account-service';
import { TitleCasePipe } from '@angular/common';

@Component({
  selector: 'app-user-list',
  imports: [UserCard, PaginationModule, FormsModule, ButtonsModule, TitleCasePipe],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList implements OnInit {
  private accountService = inject(AccountService);
  usersService = inject(UsersService);
  usersCache = inject(UsersCacheService);

  ngOnInit(): void {
    if(!this.usersCache.users().length) {
      this.onFiltersReset();
    }
  }

  onFiltersReset() {
    var user = this.accountService.currentUser();
    this.usersCache.clearFilters();
    if(user) this.usersCache.filter.update(_ => ({...FilterParams.FromOppositeGender(user!), minAge: 18, maxAge: 120 } as FilterParams));
    this.usersService.loadUsers();
  }

  onFiltersChanged() {
    this.usersCache.pagination.update(prev => setPageToOne(prev));
    this.usersService.loadUsers();
  }

  onPageChanged(event: any) {
    var pagination = this.usersCache.pagination();
    if(!pagination) return;
    if(pagination.current.pageNumber === event.page) return;
    pagination.current.pageNumber = event.page;
    this.usersCache.pagination.set(pagination);
    this.usersService.loadUsers();
  }
}
