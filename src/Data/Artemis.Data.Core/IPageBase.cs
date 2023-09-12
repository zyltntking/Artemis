namespace Artemis.Data.Core;

/// <summary>
///     分页协议接口
/// </summary>
public interface IPageBase
{
    /// <summary>
    ///     当前页码(从0开始)
    /// </summary>
    uint Page { get; set; }

    /// <summary>
    ///     页面大小
    /// </summary>
    uint Size { get; set; }
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
    T? FilterCondition { get; set; }
}

/// <summary>
///     分页响应协议接口
/// </summary>
public interface IPageReplay : IPageBase
{
    /// <summary>
    ///     过滤后数据条数
    /// </summary>
    ulong? Count { get; set; }

    /// <summary>
    ///     数据总量
    /// </summary>
    ulong? Total { get; set; }
}

/// <summary>
///     分页响应协议接口
/// </summary>
/// <typeparam name="T">响应数据</typeparam>
public interface IPageReplay<T> : IPageReplay
{
    /// <summary>
    ///     数据包
    /// </summary>
    T? Data { get; set; }
}