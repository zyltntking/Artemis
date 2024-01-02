export type EmptyResult = {}

export type DataResult<TResult> = {
    code: number,
    succeeded: boolean,
    message: string,
    error?: string,
    dateTime: string,
    timestamp: number,
    data?: TResult
}

export type StringKeySlot = KeySlot<string>;

export type NumberKeySlot = KeySlot<number>;

export type KeySlot<TKey> = {
    Id: TKey
}
