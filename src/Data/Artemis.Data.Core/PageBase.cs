namespace Artemis.Data.Core;

/// <summary>
/// 分页请求
/// </summary>
/// <typeparam name="T"></typeparam>
public class PageRequest<T> : IPageRequest<T>
{
    #region Implementation of IPageBase

    /// <summary>
    /// 当前页码(从1开始)
    /// </summary>
    public virtual uint Page { get; set; }

    /// <summary>
    /// 页面大小
    /// </summary>
    public virtual uint Size { get; set; }

    #endregion

    /// <summary>
    /// 跳过数
    /// </summary>
    public uint Skip => Page * Size;

    #region Implementation of IPageRequest<T>

    /// <summary>
    /// 过滤条件
    /// </summary>
    public virtual T? FilterCondition { get; set; }

    #endregion
}

/// <summary>
/// 分页数据响应
/// </summary>
/// <typeparam name="T">数据包</typeparam>
public class PageReply<T> : IPageReplay<T>
{
    #region Implementation of IPageBase

    /// <summary>
    /// 当前页码(从0开始)
    /// </summary>
    public virtual uint Page { get; set; }

    /// <summary>
    /// 页面大小
    /// </summary>
    public virtual uint Size { get; set; }

    #endregion

    #region Implementation of IPageReplay

    /// <summary>
    /// 过滤后数据条数
    /// </summary>
    public virtual ulong? Count { get; set; }

    /// <summary>
    /// 数据总量
    /// </summary>
    public virtual ulong? Total { get; set; }

    #endregion

    #region Implementation of IPageReplay<T>

    /// <summary>
    /// 数据包
    /// </summary>
    public virtual T? Data { get; set; }

    #endregion
}