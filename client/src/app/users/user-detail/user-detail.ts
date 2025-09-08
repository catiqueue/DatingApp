import { AfterViewInit, ChangeDetectorRef, Component, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import { User } from '../../_models/user';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { UserMessagesComponent } from "../user-messages/user-messages";
import { MessagesService } from '../../_services/messages-service';
import { LikesService } from '../../_services/likes-service';
import { PresenceService } from '../../_services/presence-service';
import { AccountService } from '../../_services/account-service';
import { HubConnectionState } from '@microsoft/signalr';

@Component({
  selector: 'app-user-detail',
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, UserMessagesComponent],
  templateUrl: './user-detail.html',
  standalone: true,
  styleUrl: './user-detail.css'
})
export class UserDetail implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild("userTabs", {static: true}) userTabs?: TabsetComponent;
  private messagesService = inject(MessagesService);
  private accountService = inject(AccountService);
  private likesService = inject(LikesService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private cdRef = inject(ChangeDetectorRef);
  protected presenceService = inject(PresenceService);
  user: User = {} as User;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  routeTabName?: string;

  ngOnInit(): void {
    this.route.data.subscribe({
      next: data => {
        this.user = data["user"];
        this.user && this.user.photos.forEach(photo => {
          this.images.push(new ImageItem({src: photo.url, thumb: photo.url}))
        });
      }
    });

    this.route.paramMap.subscribe({
      next: _ => this.onRouteChanged()
    });

    this.route.queryParams.subscribe({
      next: params => this.routeTabName = params["tab"]
    });
  }

  ngAfterViewInit(): void {
    if(this.routeTabName) this.selectTab(this.routeTabName);
  }

  public get liked(): boolean {
    return this.likesService.likedIds().includes(this.user.id);
  }

  toggleLike() {
    this.likesService.toggleLike(this.user.id).subscribe({
      next: _ => {
        if(this.liked) this.removeLike(); else this.addLike();
      }
    });
  }

  private addLike() {
    this.likesService.likedIds.update(values => values.concat(this.user.id))
  }
  private removeLike() {
    this.likesService.likedIds.update(values => values.filter(value => value !== this.user.id))
  }

  selectTab(heading: string) {
    if(!this.userTabs) return;
    const requestedTab = this.userTabs.tabs.find(tab => tab.heading === heading);
    if(!requestedTab) return;
    requestedTab.active = true;
    this.cdRef.detectChanges();
  }

  onRouteChanged() {
    const currentUser = this.accountService.currentUser();
    if(this.messagesService.connectionState === HubConnectionState.Connected
        && this.activeTab?.heading === "Messages" && currentUser) {
      this.messagesService.closeHubConnection().then(async () => {
        await this.messagesService.createHubConnection(currentUser, this.user.userName)
      });
    }
  }

  async onTabActivated(which: TabDirective) {
    this.activeTab = which;
    await this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {tab: which.heading},
      queryParamsHandling: "merge"
    });
    const currentUser = this.accountService.currentUser();
    if (this.activeTab.heading === "Messages" && this.user && currentUser) {
      await this.messagesService.createHubConnection(currentUser, this.user.userName);
    } else await this.messagesService.closeHubConnection();
  }

  ngOnDestroy(): void {
    this.messagesService.closeHubConnection();
  }
}
