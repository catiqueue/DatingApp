import { inject, Injectable, WritableSignal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Page, PaginationInfo } from '../_models/pagination';
import { MessageBoxType } from '../_models/message-box';
import { MessagesCacheService } from './cache/messages-cache';
import { appendHttpParams, readPaginatedResponse } from './pagination-utils';
import { Message } from '../_models/message';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);
  private cache = inject(MessagesCacheService);

  loadMessages() {
    this.loadMessagesInternal(this.cache.box(), this.cache.pagination()?.current);
  }

  private loadMessagesInternal(box: MessageBoxType, page?: Page) {
    var params = new HttpParams();
    if(page) params = appendHttpParams(params, page);
    params = params.append("box", box);
    readPaginatedResponse(this.http, this.baseUrl + "/messages", params, this.cache.messages, this.cache.pagination);
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(this.baseUrl + "/messages/thread/" + username);
  }

  sendMessage(username: string, content: string) {
    return this.http.post<Message>(this.baseUrl + "/messages", {recipientUsername: username, content: content});
  }

  deleteMessage(id: number) {
    return this.http.delete(this.baseUrl + "/messages/" + id);
  }
}
