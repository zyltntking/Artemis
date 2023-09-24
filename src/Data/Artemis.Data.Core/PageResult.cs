﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Artemis.Data.Core;

#region Interface

/// <summary>
///     分页协议接口
/// </summary>
public interface IPageBase
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
file interface IPageRequest<T> : IPageBase
{
    /// <summary>
    ///     过滤条件
    /// </summary>
    T Filter { get; set; }
}

/// <summary>
///     分页响应协议接口
/// </summary>
file interface IPageResult : IPageBase
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
    IEnumerable<T>? Data { get; set; }
}

#endregion

/// <summary>
///     分页请求
/// </summary>
/// <typeparam name="T"></typeparam>
[DataContract]
public sealed record PageRequest<T> : IPageRequest<T>
{
    #region Implementation of IPageRequest<T>

    /// <summary>
    ///     过滤条件
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public required T Filter { get; set; } = default!;

    #endregion

    /// <summary>
    ///     跳过数
    /// </summary>
    public int Skip => (Page - 1) * Size;

    #region Implementation of IPageBase

    /// <summary>
    ///     当前页码(从1开始)
    /// </summary>
    /// <example>1</example>
    [Required]
    [DefaultValue(1)]
    [DataMember(Order = 1)]
    public required int Page { get; set; } = 1;

    /// <summary>
    ///     页面大小
    /// </summary>
    /// <example>20</example>
    [Required]
    [DefaultValue(20)]
    [DataMember(Order = 2)]
    public required int Size { get; set; } = 20;

    #endregion
}

/// <summary>
///     分页数据响应
/// </summary>
/// <typeparam name="T">数据包</typeparam>
[DataContract]
public sealed class PageResult<T> : IPageResult<T>
{
    #region Implementation of IPageResult<T>

    /// <summary>
    ///     数据包
    /// </summary>
    [DataMember(Order = 5)]
    public IEnumerable<T>? Data { get; set; }

    #endregion

    #region Implementation of IPageBase

    /// <summary>
    ///     当前页码(从0开始)
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required int Page { get; set; }

    /// <summary>
    ///     页面大小
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required int Size { get; set; }

    #endregion

    #region Implementation of IPageResult

    /// <summary>
    ///     过滤后数据条数
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public required long Count { get; set; }

    /// <summary>
    ///     数据总量
    /// </summary>
    [Required]
    [DataMember(Order = 4)]
    public required long Total { get; set; }

    #endregion
}