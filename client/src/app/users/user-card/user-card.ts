import { Component, computed, inject, input } from '@angular/core';
import { User } from '../../_models/user';
import { RouterLink } from '@angular/router';
import { LikesService } from '../../_services/likes-service';
import { LikesCacheService } from '../../_services/cache/likes-cache';

@Component({
  selector: 'app-user-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './user-card.html',
  styleUrl: './user-card.css'
})
export class UserCard {
  private likesService = inject(LikesService);
  private likesCache = inject(LikesCacheService);
  user = input.required<User>();
  liked = computed(() => this.likesCache.likedIds().includes(this.user().id));

  toggleLike() {
    this.likesService.toggleLike(this.user().id).subscribe({
      next: (status) => {
        if(this.liked()) this.removeLike(); else this.addLike();
      }
    });
  }
  addLike() {
    this.likesCache.likedIds.update(values => values.concat(this.user().id))
  }
  removeLike() {
    this.likesCache.likedIds.update(values => values.filter(value => value !== this.user().id))
  }
}
