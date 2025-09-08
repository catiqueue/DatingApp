import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Page } from '../_models/pagination';
import { MessageBoxType } from '../_models/message-box';
import { MessagesCacheSchema, MessagesCacheService } from './cache/messages-cache';
import { readPaginatedResponse } from '../_utils/pagination-utils';
import { Message } from '../_models/message';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { appendHttpParams } from '../_utils/http-utils';
import { CacheWithGetters } from './cache/abstract-cache';
import { LoggedInUser } from '../_models/logged-in-user';
import {Group} from '../_models/group';

@Injectable({
  providedIn: 'root'
})
export class MessagesService implements CacheWithGetters<MessagesCacheSchema> {
  private apiUrl = environment.apiUrl;
  private hubUrl = environment.hubUrl;
  private http = inject(HttpClient);
  private cache = inject(MessagesCacheService);
  private hub?: HubConnection;

  public get connectionState() { return this.hub?.state; }
  public get messages() { return this.cache.messages; }
  // public get messageThreads() { return this.cache.messageThreads; }
  public get box() { return this.cache.box; }
  public get pagination() { return this.cache.pagination; }

  public async createHubConnection(user: LoggedInUser, recipient: string) {
    this.hub = new HubConnectionBuilder()
      .withUrl(this.hubUrl + "/message?user=" + recipient, {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();
    this.hub.start().catch(err => console.log(err));

    this.hub.on("OnMessageThreadReceived", messages => {
      this.messages.set(messages);
    });

    this.hub.on("OnMessageReceived", message => {
      this.messages.update(prev => [...prev, message]);
    });

    this.hub.on("OnGroupUpdated", (group: Group) => {
      if(group.connections.some(g => g.username === recipient)) {
        this.messages.update(messages => {
          messages.forEach(message => {
            if(!message.readAt) message.readAt = new Date(Date.now());
          });
          return messages;
        });
      }
    });
  }

  public async closeHubConnection() {
    if(this.hub?.state == HubConnectionState.Connected) {
      this.hub.stop().catch(err => console.log(err));
    }
  }

  public loadMessages() {
    this.loadMessagesInternal(this.cache.box(), this.cache.pagination()?.current);
  }

  private loadMessagesInternal(box: MessageBoxType, page?: Page) {
    let params = new HttpParams();
    if(page) params = appendHttpParams(params, page);
    params = params.append("box", box);
    readPaginatedResponse(this.http, this.apiUrl + "/messages", params, this.cache.messages, this.cache.pagination);
  }

  public getMessageThread(username: string) {
    return this.http.get<Message[]>(this.apiUrl + "/messages/thread/" + username);
  }

  public async sendMessage(username: string, content: string) {
    this.hub?.invoke("OnMessageSent", { recipientUsername: username, content });
  }

  public deleteMessage(id: number) {
    return this.http.delete(this.apiUrl + "/messages/" + id);
  }
}
