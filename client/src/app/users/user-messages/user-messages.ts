import { Component, inject, input, OnInit, output, ViewChild } from '@angular/core';
import { Message } from '../../_models/message';
import { MessagesService } from '../../_services/messages-service';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  selector: 'app-user-messages',
  standalone: true,
  imports: [TimeagoModule, FormsModule],
  templateUrl: './user-messages.html',
  styleUrl: './user-messages.css'
})
export class UserMessagesComponent {
  @ViewChild("messageForm") messageForm?: NgForm;
  username = input.required<string>();
  messages = input.required<Message[]>();
  messagesService = inject(MessagesService);
  messageReceivedEvent = output<Message>();
  messageContent: string = "";

  sendMessage() {
    this.messagesService.sendMessage(this.username(), this.messageContent).subscribe({
      next: message => {
        this.messageReceivedEvent.emit(message)
        this.messageForm?.reset();
      }
    });
  }
}
