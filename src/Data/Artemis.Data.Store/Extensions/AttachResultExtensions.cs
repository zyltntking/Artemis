﻿using Artemis.Data.Core;

namespace Artemis.Data.Store.Extensions;

/// <summary>
///     结果附加扩展
/// </summary>
public static class AttachResultExtensions
{
    /// <summary>
    ///     附加
    /// </summary>
    /// <typeparam name="TAttach">附加数据类型</typeparam>
    /// <param name="result">结果</param>
    /// <param name="attach">附加数据</param>
    /// <returns></returns>
    public static AttachResult<StoreResult, TAttach> Attach<TAttach>(
        this StoreResult result,
        TAttach attach)
        where TAttach : class
    {
        return new AttachResult<StoreResult, TAttach>
        {
            Result = result,
            Attach = attach
        };
    }
}