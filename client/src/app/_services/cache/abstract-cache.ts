import { signal, WritableSignal } from '@angular/core';

const SIGNAL_SYMBOL: unique symbol = (Object.getOwnPropertySymbols(signal(0))
  .find(sym => sym.toString() === "Symbol(SIGNAL)")!) as any;

function isWritableSignal<T>(v: MaybeSignal<T>): v is WritableSignal<T> {
  return (v as any)[SIGNAL_SYMBOL] !== undefined;
}

type MaybeSignal<T> = T | WritableSignal<T>;
type UnwrapSignal<T> = T extends WritableSignal<infer U> ? U : T;

function isWritableSignalOld<T>(v: MaybeSignal<T>): v is WritableSignal<T> {
  return typeof v === 'function'
  && "set" in v && typeof v.set === 'function'
  && "update" in v && typeof v.set === 'function';
}

export type CacheWithGetters<T> = {
  [K in keyof T]: T[K];
};

export abstract class AbstractCache<TSchema extends Record<keyof TSchema, MaybeSignal<any>>> {
  private readonly defaults: TSchema;
  private state: TSchema;

  constructor(schemaType: new () => TSchema) {
    this.defaults = new schemaType();
    this.state = new schemaType();
  }

  get<K extends keyof TSchema>(key: K): TSchema[K] {
    return this.state[key];
  }

  getValue<K extends keyof TSchema>(key: K): UnwrapSignal<TSchema[K]> {
    const curr = this.getInternal(this.state, key);
    return isWritableSignal(curr) ? curr() : curr;
  }

  set<K extends keyof TSchema>(key: K, value: UnwrapSignal<TSchema[K]>): void {
    const curr = this.getInternal(this.state, key);

    if(isWritableSignal(curr)) { curr.set(value); console.log(curr()); }
    else { this.state[key] = value; console.log(curr); }
  }

  reset<K extends keyof TSchema>(key: K): void {
    const defaultValue = this.getInternal(this.defaults, key);

    if(isWritableSignal(defaultValue)) { this.set(key, defaultValue()); console.log(defaultValue()); }
    else { this.set(key, defaultValue); console.log(defaultValue); }
  }

  update<K extends keyof TSchema>(key: K, updater: (current: UnwrapSignal<TSchema[K]>) => UnwrapSignal<TSchema[K]>): void {
    const curr = this.getInternal(this.state, key);

    if (isWritableSignal(curr)) { curr.update(updater); }
    else { this.state[key] = updater(curr) as TSchema[K]; }
  }

  clearAll(): void {
    console.log("clearing");
    for (const key of Object.keys(this.defaults) as (keyof TSchema)[]) {
      console.log(key);
      this.reset(key);
    }
  }

  private getInternal<K extends keyof TSchema>(from: TSchema, key: K) : MaybeSignal<UnwrapSignal<TSchema[K]>> {
    return from[key];
  }
}
