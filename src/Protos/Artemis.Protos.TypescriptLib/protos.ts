export type DataResult<TResult> = {
    code: number,
    succeeded: boolean,
    message: string,
    error?: string,
    dateTime: string,
    timestamp: number,
    data?: TResult
}

export type EmptyResult = {}
