import { Injectable, signal, WritableSignal } from "@angular/core";
import { AbstractCache, CacheWithGetters } from "./abstract-cache";
import { PaginationInfo } from "../../_models/pagination";
import { Message } from "../../_models/message";
import { MessageBoxType } from "../../_models/message-box";

export class MessagesCacheSchema {
  messages = signal<Message[]>([]);
  // messageThreads = new Map<string, WritableSignal<Message[]>>()
  pagination = signal<PaginationInfo | undefined>(undefined);
  box = signal<MessageBoxType>(MessageBoxType.Unread);
}

@Injectable({ providedIn: 'root' })
export class MessagesCacheService extends AbstractCache<MessagesCacheSchema> implements CacheWithGetters<MessagesCacheSchema> {
  constructor() { super(MessagesCacheSchema); }
  public get messages() { return this.get("messages"); }
  // public get messageThreads() { return this.get("messageThreads"); }
  public get pagination() { return this.get("pagination"); }
  public get box() { return this.get("box"); }

  clearFilters() {
    this.reset("box");
    this.reset("pagination");
  }
}
