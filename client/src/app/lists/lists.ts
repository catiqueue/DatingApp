import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikesService } from '../_services/likes-service';
import { LikedListType } from '../_models/likes-request';
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { FormsModule } from '@angular/forms';
import { UserCard } from "../users/user-card/user-card";
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { setPageToOne } from '../_utils/pagination-utils';

@Component({
  selector: 'app-lists',
  imports: [ButtonsModule, FormsModule, UserCard, PaginationModule],
  templateUrl: './lists.html',
  standalone: true,
  styleUrl: './lists.css'
})
export class Lists implements OnInit, OnDestroy {
  protected likesService = inject(LikesService);

  ngOnInit(): void {
    if(!this.likesService.users()?.length) {
      this.likesService.pagination.set(undefined);
      this.likesService.loadLikedList();
    }
  }

  ngOnDestroy(): void {
    this.likesService.users.set([]);
  }

  getTitle() {
    switch(this.likesService.predicate()) {
      case LikedListType.Liked: return "Users who you liked"
      case LikedListType.LikedBy: return "Users who liked you"
      case LikedListType.Mutual: return "Users who you liked and they liked you"
    }
  }

  onFilterChanged() {
    this.likesService.pagination.update(prev => setPageToOne(prev));
    this.likesService.loadLikedList();
  }

  onPageChanged(event: any) {
    var pagination = this.likesService.pagination();
    if(!pagination) return;
    if(pagination.current.pageNumber === event.page) return;
    pagination.current.pageNumber = event.page;
    this.likesService.pagination.set(pagination);
    this.likesService.loadLikedList();
  }
}
