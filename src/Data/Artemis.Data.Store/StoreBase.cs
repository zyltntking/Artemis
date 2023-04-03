using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
/// 抽象基本存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public abstract class StoreBase<TEntity, TKey> : IStoreBase<TEntity, TKey> where TEntity : IModelBase<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// 创建一个新的基本存储实例
    /// </summary>
    /// <param name="describer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected StoreBase(IStoreErrorDescriber describer)
    {
        ErrorDescriber = describer ?? throw new ArgumentNullException(nameof(describer));
    }

    /// <summary>
    /// 已释放标识
    /// </summary>
    private bool _disposed;

    /// <summary>
    /// 设置当前发生错误的错误描述者
    /// </summary>
    public IStoreErrorDescriber ErrorDescriber { get; set; }

    #region Implementation of IStoreBase<in TEntity,TKey>

    /// <summary>
    /// 获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id</returns>
    public virtual TKey GetId(TEntity entity)
    {
        ThrowIfDisposed();
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        return entity.Id;
    }

    /// <summary>
    /// 获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作的信号</param>
    /// <returns>Id</returns>
    public virtual Task<TKey> GetIdAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        return Task.FromResult(entity.Id);
    }

    /// <summary>
    /// 获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id字符串</returns>
    public string GetIdString(TEntity entity)
    {
        ThrowIfDisposed();
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        return ConvertIdToString(entity.Id)!;
    }

    /// <summary>
    /// 获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作信号</param>
    /// <returns></returns>
    public virtual Task<string> GetIdStringAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        return Task.FromResult(ConvertIdToString(entity.Id)!);
    }

    /// <summary>
    /// 是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>判断结果</returns>
    public virtual bool IsDeleted(TEntity entity)
    {
        ThrowIfDisposed();
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        return entity.DeletedAt == null;
    }

    /// <summary>
    /// 是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>判断结果</returns>
    public virtual Task<bool> IsDeletedAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }
        return Task.FromResult(entity.DeletedAt == null);
    }

    #endregion

    /// <summary>
    /// 转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    protected virtual TKey? ConvertIdFromString(string? id)
    {
        return id.IdFromString<TKey>();
    }

    /// <summary>
    /// 转换Id为字符串
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>字符串</returns>
    protected virtual string? ConvertIdToString(TKey id)
    {
        return id.IdToString();
    }

    /// <summary>
    /// Throws if this class has been disposed.
    /// </summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(GetType().Name);
        }
    }

    #region Implementation of IDisposable

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _disposed = true;
    }

    #endregion
}