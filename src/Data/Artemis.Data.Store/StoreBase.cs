using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;

namespace Artemis.Data.Store;

/// <summary>
///     抽象基本存储实现
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public abstract class StoreBase<TEntity, TKey> : IStoreBase<TEntity, TKey>
    where TEntity : IModelBase<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     已释放标识
    /// </summary>
    private bool _disposed;

    /// <summary>
    ///     创建一个新的基本存储实例
    /// </summary>
    /// <param name="describer"></param>
    /// <exception cref="StoreParameterNullException"></exception>
    protected StoreBase(IStoreErrorDescriber describer)
    {
        ErrorDescriber = describer ?? throw new StoreParameterNullException(nameof(describer));
    }

    #region Implementation of IDisposable

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    #endregion

    /// <summary>
    ///     设置当前发生错误的错误描述者
    /// </summary>
    protected IStoreErrorDescriber ErrorDescriber { get; set; }

    /// <summary>
    ///     转换字符串到id
    /// </summary>
    /// <param name="id">id字符串</param>
    /// <returns>id</returns>
    protected virtual TKey? ConvertIdFromString(string? id)
    {
        return id.IdFromString<TKey>();
    }

    /// <summary>
    ///     转换Id为字符串
    /// </summary>
    /// <param name="id">id</param>
    /// <returns>字符串</returns>
    protected virtual string? ConvertIdToString(TKey id)
    {
        return id.IdToString();
    }

    /// <summary>
    ///     Throws if this class has been disposed.
    /// </summary>
    /// <exception cref="StoreDisposedException"></exception>
    protected void ThrowIfDisposed()
    {
        if (_disposed) throw new StoreDisposedException(GetType());
    }

    #region Implementation of IStoreBase<in TEntity,TKey>

    /// <summary>
    ///     获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id</returns>
    public TKey GetId(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        return entity.Id;
    }

    /// <summary>
    ///     获取指定实体Id
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作的信号</param>
    /// <returns>Id</returns>
    public Task<TKey> GetIdAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        return Task.FromResult(entity.Id);
    }

    /// <summary>
    ///     获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>Id字符串</returns>
    public string GetIdString(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        return ConvertIdToString(entity.Id)!;
    }

    /// <summary>
    ///     获取指定实体Id字符串
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">取消异步操作信号</param>
    /// <returns></returns>
    public Task<string> GetIdStringAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        return Task.FromResult(ConvertIdToString(entity.Id)!);
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <returns>判断结果</returns>
    public bool IsDeleted(TEntity entity)
    {
        OnActionExecuting(entity, nameof(entity));
        return entity.DeletedAt == null;
    }

    /// <summary>
    ///     是否被删除
    /// </summary>
    /// <param name="entity">实体</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>判断结果</returns>
    public Task<bool> IsDeletedAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(entity, nameof(entity), cancellationToken);
        return Task.FromResult(entity.DeletedAt == null);
    }

    #endregion

    #region OnActionExecution

    /// <summary>
    ///     方法执行前
    /// </summary>
    /// <param name="value">实体</param>
    /// <param name="name">参数名</param>
    /// <exception cref="StoreParameterNullException">空参数异常</exception>
    protected void OnActionExecuting<TValue>(TValue value, string name)
    {
        ThrowIfDisposed();
        if (value == null) throw new StoreParameterNullException(name);
    }

    /// <summary>
    ///     异步方法执行前
    /// </summary>
    /// <param name="value">实体</param>
    /// <param name="name">参数名</param>
    /// <param name="cancellationToken">取消信号</param>
    protected void OnAsyncActionExecuting<TValue>(TValue value, string name,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        OnActionExecuting(value, name);
    }

    #endregion
}