using System.Runtime.Serialization;

namespace Artemis.Data.Core;

/// <summary>
/// 分页请求
/// </summary>
/// <typeparam name="T"></typeparam>
[DataContract]
public sealed class PageRequest<T> : IPageRequest<T>
{
    #region Implementation of IPageBase

    /// <summary>
    /// 当前页码(从1开始)
    /// </summary>
    [DataMember(Order = 1)]
    public uint Page { get; set; }

    /// <summary>
    /// 页面大小
    /// </summary>
    [DataMember(Order = 2)]
    public uint Size { get; set; }

    #endregion

    /// <summary>
    /// 跳过数
    /// </summary>
    public uint Skip => Page * Size;

    #region Implementation of IPageRequest<T>

    /// <summary>
    /// 过滤条件
    /// </summary>
    [DataMember(Order = 3)]
    public T? FilterCondition { get; set; }

    #endregion
}

/// <summary>
/// 分页数据响应
/// </summary>
/// <typeparam name="T">数据包</typeparam>
[DataContract]
public sealed class PageReply<T> : IPageReplay<T>
{
    #region Implementation of IPageBase

    /// <summary>
    /// 当前页码(从0开始)
    /// </summary>
    [DataMember(Order = 1)]
    public uint Page { get; set; }

    /// <summary>
    /// 页面大小
    /// </summary>
    [DataMember(Order = 2)]
    public uint Size { get; set; }

    #endregion

    #region Implementation of IPageReplay

    /// <summary>
    /// 过滤后数据条数
    /// </summary>
    [DataMember(Order = 3)]
    public ulong? Count { get; set; }

    /// <summary>
    /// 数据总量
    /// </summary>
    [DataMember(Order = 4)]
    public ulong? Total { get; set; }

    #endregion

    #region Implementation of IPageReplay<T>

    /// <summary>
    /// 数据包
    /// </summary>
    [DataMember(Order = 5)]
    public T? Data { get; set; }

    #endregion
}