import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikesService } from '../_services/likes-service';
import { User } from '../_models/user';
import { LikedListType } from '../_models/likes-request';
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { FormsModule } from '@angular/forms';
import { UserCard } from "../users/user-card/user-card";
import { LikesCacheService } from '../_services/cache/likes-cache';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-lists',
  imports: [ButtonsModule, FormsModule, UserCard, PaginationModule],
  templateUrl: './lists.html',
  styleUrl: './lists.css'
})
export class Lists implements OnInit, OnDestroy {
  private likesService = inject(LikesService);
  protected likesCache = inject(LikesCacheService);

  ngOnInit(): void {
    if(!this.likesCache.users()?.length) {
      this.likesCache.pagination.set(undefined);
      this.likesService.loadLikedList();
    }
  }

  ngOnDestroy(): void {
    this.likesCache.users.set([]);
  }

  getTitle() {
    switch(this.likesCache.predicate()) {
      case LikedListType.Liked: return "Users who you liked"
      case LikedListType.LikedBy: return "Users who liked you"
      case LikedListType.Mutual: return "Users who you liked and they liked you"
    }
  }

  loadList() {
    this.likesService.loadLikedList();
  }

  pageChanged(event: any) {
    var pagination = this.likesCache.pagination();
    if(!pagination) return;
    if(pagination.current.pageNumber === event.page) return;
    pagination.current.pageNumber = event.page;
    this.likesCache.pagination.set(pagination);
    this.likesService.loadLikedList();
  }
}
