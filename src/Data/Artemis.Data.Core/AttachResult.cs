﻿using System.ComponentModel.DataAnnotations;

namespace Artemis.Data.Core;

#region Interface

/// <summary>
///     结果数据附加集合
/// </summary>
/// <typeparam name="TResult">结果</typeparam>
/// <typeparam name="TAttach">附加数据</typeparam>
file interface IAttachResult<TResult, TAttach>
    where TResult : class
    where TAttach : class
{
    /// <summary>
    ///     结果
    /// </summary>
    public TResult Result { get; init; }

    /// <summary>
    ///     附加数据
    /// </summary>
    public TAttach Attach { get; init; }
}

#endregion

/// <summary>
///     结果数据附加集合记录
/// </summary>
/// <typeparam name="TResult">结果</typeparam>
/// <typeparam name="TAttach">附加</typeparam>
public record AttachResult<TResult, TAttach> : IAttachResult<TResult, TAttach>
    where TResult : class
    where TAttach : class
{
    /// <summary>
    ///     结果
    /// </summary>
    [Required]
    public required TResult Result { get; init; }

    /// <summary>
    ///     附加数据
    /// </summary>
    [Required]
    public required TAttach Attach { get; init; }
}