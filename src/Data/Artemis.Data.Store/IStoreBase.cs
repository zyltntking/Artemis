using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
///     可映射存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IStoreBase<in TEntity> : IStoreBase<TEntity, Guid> where TEntity : IModelBase<Guid>
{
}

/// <summary>
///     基本存储接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IStoreBase<in TEntity, TKey> : IDisposable
    where TEntity : IModelBase<TKey> 
    where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id</returns>
    TKey GetId(TEntity entity);

    /// <summary>
    ///     获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作的信号</param>
    /// <returns>Id</returns>
    Task<TKey> GetIdAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id字符串</returns>
    string GetIdString(TEntity entity);

    /// <summary>
    ///     获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作信号</param>
    /// <returns>Id字符串</returns>
    Task<string> GetIdStringAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>判断结果</returns>
    bool IsDeleted(TEntity entity);

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>判断结果</returns>
    Task<bool> IsDeletedAsync(
        TEntity entity, 
        CancellationToken cancellationToken = default);
}