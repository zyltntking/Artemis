using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Artemis.Data.Core;

/// <summary>
///     分页请求
/// </summary>
/// <typeparam name="T"></typeparam>
[DataContract]
public sealed class PageRequest<T> : IPageRequest<T>
{
    #region Implementation of IPageRequest<T>

    /// <summary>
    ///     过滤条件
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public T Filter { get; set; } = default!;

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
    public int Page { get; set; } = 1;

    /// <summary>
    ///     页面大小
    /// </summary>
    /// <example>20</example>
    [Required]
    [DefaultValue(20)]
    [DataMember(Order = 2)]
    public int Size { get; set; } = 20;

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
    [DataMember(Order = 1)]
    public int Page { get; set; }

    /// <summary>
    ///     页面大小
    /// </summary>
    [DataMember(Order = 2)]
    public int Size { get; set; }

    #endregion

    #region Implementation of IPageResult

    /// <summary>
    ///     过滤后数据条数
    /// </summary>
    [DataMember(Order = 3)]
    public long Count { get; set; }

    /// <summary>
    ///     数据总量
    /// </summary>
    [DataMember(Order = 4)]
    public long Total { get; set; }

    #endregion
}