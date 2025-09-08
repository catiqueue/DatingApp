import { Component, inject, OnInit } from '@angular/core';
import { UsersService } from '../../_services/users-service';
import { UserCard } from "../user-card/user-card";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { setPageToOne } from '../../_utils/pagination-utils';
import { FilterParams } from '../../_models/filer-params';
import { AccountService } from '../../_services/account-service';

@Component({
  selector: 'app-user-list',
  imports: [UserCard, PaginationModule, FormsModule, ButtonsModule],
  templateUrl: './user-list.html',
  standalone: true,
  styleUrl: './user-list.css'
})
export class UserList implements OnInit {
  private accountService = inject(AccountService);
  usersService = inject(UsersService);

  ngOnInit(): void {
    if(!this.usersService.users().length) {
      this.onFiltersReset();
    }
  }

  onFiltersReset() {
    var user = this.accountService.currentUser();
    this.usersService.clearFilters();
    if(user) this.usersService.filter.update(_ => ({...FilterParams.FromOppositeGender(user!), minAge: 18, maxAge: 120 } as FilterParams));
    this.usersService.loadUsers();
  }

  onFiltersChanged() {
    this.usersService.pagination.update(prev => setPageToOne(prev));
    this.usersService.loadUsers();
  }

  onPageChanged(event: any) {
    var pagination = this.usersService.pagination();
    if(!pagination) return;
    if(pagination.current.pageNumber === event.page) return;
    pagination.current.pageNumber = event.page;
    this.usersService.pagination.set(pagination);
    this.usersService.loadUsers();
  }
}
