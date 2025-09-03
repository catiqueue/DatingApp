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
      this.messagesService.getMessageThread(this.user.username).subscribe({
        next: response => this.messages = response
      });
    }
  }

  onMessageReceived(message: Message) {
    this.messages.push(message);
  }

  /* loadUser() {
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
  } */
}
