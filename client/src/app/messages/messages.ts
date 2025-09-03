import { Component, inject, OnInit } from '@angular/core';
import { MessagesService } from '../_services/messages-service';
import { MessagesCacheService } from '../_services/cache/messages-cache';
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MessageBoxType } from '../_models/message-box';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from '../_models/message';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-messages',
  imports: [ButtonsModule, FormsModule, PaginationModule, TimeagoModule, RouterLink, PaginationModule],
  templateUrl: './messages.html',
  styleUrl: './messages.css'
})
export class Messages implements OnInit {
  protected messagesService = inject(MessagesService);
  protected messagesCache = inject(MessagesCacheService);
  isOutbox = this.messagesCache.box() === MessageBoxType.Outbox;

  ngOnInit(): void {
    this.messagesService.loadMessages();
  }

  /* isOutbox(box: MessageBoxType) {
    return box === MessageBoxType.Outbox;
  } */

  deleteMessage(id: number) {
    this.messagesService.deleteMessage(id).subscribe({
      next: _ => {
        this.messagesCache.messages.update(arr => arr.filter(mess => mess.id != id));
        
      }
    });
  }

  getRoute(message: Message) {
    return "/users/" + (this.isOutbox
      ? message.recipientUsername
      : message.senderUsername);
  }

  pageChanged(event: any) {
    var pagination = this.messagesCache.pagination();
    if(!pagination) return;
    if(pagination.current.pageNumber === event.page) return;
    pagination.current.pageNumber = event.page;
    this.messagesCache.pagination.set(pagination);
    this.messagesService.loadMessages();
  }
}
