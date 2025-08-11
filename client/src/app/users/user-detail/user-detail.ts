import { Component, inject, OnInit } from '@angular/core';
import { UsersService } from '../../_services/users-service';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../_models/user';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';

@Component({
  selector: 'app-user-detail',
  imports: [TabsModule, GalleryModule],
  templateUrl: './user-detail.html',
  styleUrl: './user-detail.css'
})
export class UserDetail implements OnInit {
  private userService = inject(UsersService);
  private route = inject(ActivatedRoute);
  user: User | null = null;
  images: GalleryItem[] = [];

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    const username = this.route.snapshot.paramMap.get("username");
    if(!username) return;
    this.userService.getUserByUsername(username).subscribe({
      next: user => {
        this.user = user;
        user.photos.forEach(photo => {
          this.images.push(new ImageItem({src: photo.url, thumb: photo.url}))
        })
      }
    });
  }
}
