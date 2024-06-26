﻿using System.ComponentModel.DataAnnotations;

namespace Artemis.Data.Core;

#region Interface

/// <summary>
///     分页协议接口
/// </summary>
internal interface IPageSlot
{
    /// <summary>
    ///     当前页码(从0开始)
    /// </summary>
    int Page { get; set; }

    /// <summary>
    ///     页面大小
    /// </summary>
    int Size { get; set; }
}

/// <summary>
///     分页请求协议接口
/// </summary>
/// <typeparam name="T">过滤条件</typeparam>
file interface IPageRequest<T> : IPageSlot
{
    /// <summary>
    ///     过滤条件
    /// </summary>
    T Filter { get; set; }
}

/// <summary>
///     分页响应协议接口
/// </summary>
file interface IPageResult : IPageSlot
{
    /// <summary>
    ///     过滤后数据条数
    /// </summary>
    long Count { get; set; }

    /// <summary>
    ///     数据总量
    /// </summary>
    long Total { get; set; }
}

/// <summary>
///     分页响应协议接口
/// </summary>
/// <typeparam name="T">响应数据</typeparam>
file interface IPageResult<T> : IPageResult
{
    /// <summary>
    ///     数据包
    /// </summary>
    IEnumerable<T>? Items { get; set; }
}

#endregion

/// <summary>
///     基本分页处理
/// </summary>
public abstract record PageSlot : IPageSlot
{
    /// <summary>
    ///     跳过数
    /// </summary>
    public int Skip => (Page - 1) * Size;

    #region Implementation of IPageSlot

    /// <summary>
    ///     当前页码(从1开始)
    /// </summary>
    /// <example>1</example>
    [Required]
    public virtual required int Page { get; set; } = 1;

    /// <summary>
    ///     页面大小
    /// </summary>
    /// <example>20</example>
    [Required]
    public virtual required int Size { get; set; } = 20;

    #endregion
}

/// <summary>
///     分页请求
/// </summary>
/// <typeparam name="T"></typeparam>
public record PageRequest<T> : PageSlot, IPageRequest<T>
{
    #region Implementation of IPageRequest<T>

    /// <summary>
    ///     过滤条件
    /// </summary>
    [Required]
    public virtual required T Filter { get; set; } = default!;

    #endregion
}

/// <summary>
///     分页数据结果
/// </summary>
public abstract record AbstractPageResult : PageSlot, IPageResult
{
    #region Implementation of IPageResult

    /// <summary>
    ///     过滤后数据条数
    /// </summary>
    [Required]
    public virtual required long Count { get; set; }

    /// <summary>
    ///     数据总量
    /// </summary>
    [Required]
    public virtual required long Total { get; set; }

    #endregion
}

/// <summary>
///     分页数据响应
/// </summary>
/// <typeparam name="T">数据包</typeparam>
public sealed record PageResult<T> : AbstractPageResult, IPageResult<T>
{
    #region Implementation of IPageResult<T>

    /// <summary>
    ///     数据包
    /// </summary>
    public IEnumerable<T>? Items { get; set; }

    #endregion
}