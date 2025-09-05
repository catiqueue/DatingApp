import { AfterContentInit, AfterViewChecked, AfterViewInit, ChangeDetectorRef, Component, inject, OnInit, ViewChild } from '@angular/core';
import { UsersService } from '../../_services/users-service';
import { ActivatedRoute } from '@angular/router';
import { User } from '../../_models/user';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule, TimeagoPipe } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { UserMessagesComponent } from "../user-messages/user-messages";
import { Message } from '../../_models/message';
import { MessagesService } from '../../_services/messages-service';
import { LikesService } from '../../_services/likes-service';
import { LikesCacheService } from '../../_services/cache/likes-cache';

@Component({
  selector: 'app-user-detail',
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, UserMessagesComponent],
  templateUrl: './user-detail.html',
  styleUrl: './user-detail.css'
})
export class UserDetail implements OnInit, AfterViewInit {
  @ViewChild("userTabs", {static: true}) userTabs?: TabsetComponent;
  private userService = inject(UsersService);
  private messagesService = inject(MessagesService);
  private likesService = inject(LikesService);
  private likesCache = inject(LikesCacheService);
  private route = inject(ActivatedRoute);
  private cdRef = inject(ChangeDetectorRef);
  user: User = {} as User;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  routeTabName?: string;
  messages: Message[] = [];

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => {
        this.user = data["user"];
        this.user && this.user.photos.forEach(photo => {
          this.images.push(new ImageItem({src: photo.url, thumb: photo.url}))
        });
      }
    });

    this.route.queryParams.subscribe({
      next: params => this.routeTabName = params["tab"]
    });
  }

  ngAfterViewInit(): void {
    if(this.routeTabName) this.selectTab(this.routeTabName);
  }


  public get liked(): boolean {
    return this.likesCache.likedIds().includes(this.user.id);
  }

  toggleLike() {
    this.likesService.toggleLike(this.user.id).subscribe({
      next: _ => {
        if(this.liked) this.removeLike(); else this.addLike();
      }
    });
  }
  private addLike() {
    this.likesCache.likedIds.update(values => values.concat(this.user.id))
  }
  private removeLike() {
    this.likesCache.likedIds.update(values => values.filter(value => value !== this.user.id))
  }


  selectTab(heading: string) {
    if(!this.userTabs) return;
    var requestedTab = this.userTabs.tabs.find(tab => tab.heading === heading);
    if(!requestedTab) return;
    requestedTab.active = true;
    this.cdRef.detectChanges();
  }

  onTabActivated(which: TabDirective) {
    this.activeTab = which;
    if(this.activeTab.heading === "Messages" && !this.messages.length && this.user) {
      this.messagesService.getMessageThread(this.user.userName).subscribe({
        next: response => this.messages = response
      });
    }
  }

  onMessageReceived(message: Message) {
    this.messages.push(message);
  }
}
