using System.Runtime.Serialization;

namespace Artemis.Data.Core;

/// <summary>
///     分页请求
/// </summary>
/// <typeparam name="T"></typeparam>
[DataContract]
public sealed class PageRequest<T> : IPageRequest<T>
{
    /// <summary>
    ///     跳过数
    /// </summary>
    public int Skip => Page * Size;

    #region Implementation of IPageRequest<T>

    /// <summary>
    ///     过滤条件
    /// </summary>
    [DataMember(Order = 3)]
    public T? FilterCondition { get; set; }

    #endregion

    #region Implementation of IPageBase

    /// <summary>
    ///     当前页码(从1开始)
    /// </summary>
    [DataMember(Order = 1)]
    public int Page { get; set; }

    /// <summary>
    ///     页面大小
    /// </summary>
    [DataMember(Order = 2)]
    public int Size { get; set; }

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