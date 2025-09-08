import { Component, inject, input, ViewChild } from '@angular/core';
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
  public username = input.required<string>();
  protected messagesService = inject(MessagesService);
  protected messageContent: string = "";

  public sendMessage() {
    this.messagesService.sendMessage(this.username(), this.messageContent)
      .then(() => this.messageForm?.reset());
  }
}
