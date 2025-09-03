import { Injectable, WritableSignal } from "@angular/core";
import { AbstractCache } from "./abstract-cache";
import { PaginationInfo } from "../../_models/pagination";
import { Message } from "../../_models/message";
import { MessageBoxType } from "../../_models/message-box";

export interface MessagesCacheSchema {
  messages: Message[];
  pagination: PaginationInfo | undefined;
  box: MessageBoxType;
}

@Injectable({ providedIn: 'root' })
export class MessagesCacheService extends AbstractCache<MessagesCacheSchema> {
  public get box(): WritableSignal<MessageBoxType> {
    return this.getOrCreate("box", () => MessageBoxType.Unread);
  }
  public get messages(): WritableSignal<Message[]> {
    return this.getOrCreate("messages", () => []);
  }
  public get pagination(): WritableSignal<PaginationInfo | undefined> {
    return this.getOrCreate("pagination", () => undefined);
  }

  clearFilters() {
    this.box.set(MessageBoxType.Unread);
    this.pagination.set(undefined);
  }

  clearAll() {
    this.clearFilters();
    this.messages.set([]);
  }
}
