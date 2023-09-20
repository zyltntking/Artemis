namespace Artemis.Data.Core;

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
public interface IPageRequest<T> : IPageBase
{
    /// <summary>
    ///     过滤条件
    /// </summary>
    T Filter { get; set; }
}

/// <summary>
///     分页响应协议接口
/// </summary>
public interface IPageResult : IPageBase
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
public interface IPageResult<T> : IPageResult
{
    /// <summary>
    ///     数据包
    /// </summary>
    IEnumerable<T>? Data { get; set; }
}