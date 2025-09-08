import {AfterViewChecked, Component, inject, input, ViewChild} from '@angular/core';
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
export class UserMessagesComponent implements AfterViewChecked {
  @ViewChild("messageForm") messageForm?: NgForm;
  @ViewChild("scrollable") scrollable?: any;
  public username = input.required<string>();
  protected messagesService = inject(MessagesService);
  protected messageContent: string = "";

  public sendMessage() {
    this.messagesService.sendMessage(this.username(), this.messageContent)
      .then(() => {
        this.messageForm?.reset();
        this.scrollToBottom();
      });
  }

  public ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  private scrollToBottom() {
    if (this.scrollable) {
      this.scrollable.nativeElement.scrollTop = this.scrollable.nativeElement.scrollHeight;
    }
  }
}
