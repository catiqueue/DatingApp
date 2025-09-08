import { Component, computed, inject, input } from '@angular/core';
import { User } from '../../_models/user';
import { RouterLink } from '@angular/router';
import { LikesService } from '../../_services/likes-service';
import { PresenceService } from '../../_services/presence-service';

@Component({
  selector: 'app-user-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './user-card.html',
  styleUrl: './user-card.css'
})
export class UserCard {
  private likesService = inject(LikesService);
  private presenceService = inject(PresenceService);
  user = input.required<User>();
  isLiked = computed(() => this.likesService.likedIds().includes(this.user().id));
  isOnline = computed(() => this.presenceService.onlineUsers().includes(this.user().userName));

  toggleLike() {
    this.likesService.toggleLike(this.user().id).subscribe({
      next: () => {
        if(this.isLiked()) this.removeLike(); else this.addLike();
      }
    });
  }
  addLike() {
    this.likesService.likedIds.update(values => values.concat(this.user().id))
  }
  removeLike() {
    this.likesService.likedIds.update(values => values.filter(value => value !== this.user().id))
  }
}
