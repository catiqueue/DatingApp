import { Component, inject, OnInit } from '@angular/core';
import { MessagesService } from '../_services/messages-service';
import { ButtonsModule } from "ngx-bootstrap/buttons";
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MessageBoxType } from '../_models/message-box';
import { TimeagoModule } from 'ngx-timeago';
import { Message } from '../_models/message';
import { RouterLink } from '@angular/router';
import { setPageToOne } from '../_utils/pagination-utils';

@Component({
  selector: 'app-messages',
  imports: [ButtonsModule, FormsModule, PaginationModule, TimeagoModule, RouterLink, PaginationModule],
  templateUrl: './messages.html',
  standalone: true,
  styleUrl: './messages.css'
})
export class Messages implements OnInit {
  protected messagesService = inject(MessagesService)
  isOutbox = this.messagesService.box() === MessageBoxType.Outbox;

  ngOnInit(): void {
    this.messagesService.loadMessages();
  }

  deleteMessage(id: number) {
    this.messagesService.deleteMessage(id).subscribe({
      next: _ => {
        this.messagesService.messages.update(arr => arr.filter(mess => mess.id != id));

      }
    });
  }

  getRoute(message: Message) {
    return "/users/" + (this.isOutbox
      ? message.recipientUsername
      : message.senderUsername);
  }

  onFilterChanged() {
    this.messagesService.pagination.update(prev => setPageToOne(prev));
    this.messagesService.loadMessages();
  }

  onPageChanged(event: any) {
    var pagination = this.messagesService.pagination();
    if(!pagination) return;
    if(pagination.current.pageNumber === event.page) return;
    pagination.current.pageNumber = event.page;
    this.messagesService.pagination.set(pagination);
    this.messagesService.loadMessages();
  }
}
