import { WritableSignal, signal } from '@angular/core';

// what
export abstract class AbstractCache<TSchema extends Record<string, any>> {
  private map = new Map<keyof TSchema, WritableSignal<TSchema[keyof TSchema]>>();

  getOrCreate<K extends keyof TSchema>(key: K, factory: () => TSchema[K]): WritableSignal<TSchema[K]> {
    if(!this.map.get(key)) this.map.set(key, signal(factory()));
    return this.getSignal(key)!;
  }

  getSignal<K extends keyof TSchema>(key: K): WritableSignal<TSchema[K]> | undefined {
    // is it the compiler that's stupid or i am?
    return this.map.get(key) as WritableSignal<TSchema[K]> | undefined;
  }

  getValue<K extends keyof TSchema>(key: K): TSchema[K] | undefined {
    var sig = this.getSignal(key);
    return sig ? sig() : undefined;
  }

  set<K extends keyof TSchema>(key: K, value: TSchema[K]) {
    var sig = this.getSignal(key);
    if(sig) sig.set(value);
    else this.map.set(key, signal(value));
  }

  update<K extends keyof TSchema>(key: K, updater: Partial<TSchema[K]> | ((current: TSchema[K]) => TSchema[K])) {
    var signal = this.getSignal(key);
    if(!signal) return;
    var current = signal();

    if (typeof updater === "function") {
      signal.set(updater(current));
    } else if (current !== undefined) {
      signal.set({ ...current, ...updater } as TSchema[K]);
    } else {
      signal.set(updater as TSchema[K]);
    }
  }
}
