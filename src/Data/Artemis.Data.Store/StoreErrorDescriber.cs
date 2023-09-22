using System.Runtime.Serialization;

namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     存储异常描述接口
/// </summary>
file interface IStoreErrorDescriber
{
    /// <summary>
    ///     生成默认异常
    /// </summary>
    /// <returns></returns>
    StoreError DefaultError();

    /// <summary>
    ///     提示并发失败异常
    /// </summary>
    /// <returns></returns>
    StoreError ConcurrencyFailure();

    /// <summary>
    ///     提示未找到Id失败
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    StoreError NotFoundId(string? id);

    /// <summary>
    ///     提示已允许具缓存策略
    /// </summary>
    /// <returns></returns>
    StoreError EnableCache();

    /// <summary>
    ///     实体未找到
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    StoreError EntityNotFound(string? entity, string? flag);

    /// <summary>
    ///     实体已存在
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    StoreError EntityHasBeenSet(string? entity, string? flag);
}

/// <summary>
///     存储子系统错误接口
/// </summary>
file interface IStoreError
{
    /// <summary>
    ///     错误编码
    /// </summary>
    string? Code { get; init; }

    /// <summary>
    ///     错误描述
    /// </summary>
    string? Description { get; init; }
}

#endregion

/// <summary>
///     存储异常描述器
/// </summary>
public sealed record StoreErrorDescriber : IStoreErrorDescriber
{
    /// <summary>
    ///     生成默认异常
    /// </summary>
    /// <returns>默认异常</returns>
    public StoreError DefaultError()
    {
        return new StoreError
        {
            Code = nameof(DefaultError),
            Description = Resources.DefaultError
        };
    }

    /// <summary>
    ///     提示发生并发失败
    /// </summary>
    /// <returns></returns>
    public StoreError ConcurrencyFailure()
    {
        return new StoreError
        {
            Code = nameof(ConcurrencyFailure),
            Description = Resources.ConcurrencyFailure
        };
    }

    /// <summary>
    ///     提示未找到Id失败
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public StoreError NotFoundId(string? id)
    {
        return new StoreError
        {
            Code = nameof(NotFoundId),
            Description = Formatter.FormatNotFoundId(id)
        };
    }

    /// <summary>
    ///     提示已允许具缓存策略
    /// </summary>
    /// <returns></returns>
    public StoreError EnableCache()
    {
        return new StoreError
        {
            Code = nameof(EnableCache),
            Description = Resources.EnableCache
        };
    }

    /// <summary>
    ///     实体未找到
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public StoreError EntityNotFound(string? entity, string? flag)
    {
        return new StoreError
        {
            Code = nameof(EntityNotFound),
            Description = Formatter.FormatEntityNotFound(entity, flag)
        };
    }

    /// <summary>
    ///     实体已存在
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public StoreError EntityHasBeenSet(string? entity, string? flag)
    {
        return new StoreError
        {
            Code = nameof(EntityHasBeenSet),
            Description = Formatter.FormatEntityHasBeenSet(entity, flag)
        };
    }
}

/// <summary>
///     存储子系统错误封装
/// </summary>
[DataContract]
public record StoreError : IStoreError
{
    /// <summary>
    ///     错误编码
    /// </summary>
    [DataMember(Order = 1)]
    public string? Code { get; init; }

    /// <summary>
    ///     错误描述
    /// </summary>
    [DataMember(Order = 2)]
    public string? Description { get; init; }
}