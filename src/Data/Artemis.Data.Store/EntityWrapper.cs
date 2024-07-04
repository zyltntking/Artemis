using Artemis.Data.Core;
using Artemis.Data.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Artemis.Data.Store;

internal class EntityWrapper<TDbContext, TEntity> where TDbContext : DbContext
    where TEntity : class
{
    /// <summary>
    ///     实体包装类
    /// </summary>
    /// <param name="context"></param>
    /// <param name="proxy"></param>
    /// <param name="logger"></param>
    public EntityWrapper(
        TDbContext context,
        IEntityProxy proxy,
        ILogger? logger = null)
    {
        Context = context;
        Logger = logger;
        Describer = new StoreErrorDescriber();
    }

    #region PropertyAccess

    /// <summary>
    ///     数据访问上下文
    /// </summary>
    private DbContext Context { get; }

    /// <summary>
    ///     日志依赖
    /// </summary>
    private ILogger? Logger { get; }

    /// <summary>
    ///     设置当前发生错误的错误描述者
    /// </summary>
    private StoreErrorDescriber Describer { get; }

    #endregion

    #region SettingAccessor

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    private bool DebugLogger => throw new NotImplementedException();

    /// <summary>
    ///     设置是否自动保存更改
    /// </summary>
    private bool AutoSaveChanges => throw new NotImplementedException();

    /// <summary>
    ///     元数据托管标识
    /// </summary>
    private bool MetaDataHosting => throw new NotImplementedException();

    /// <summary>
    ///     软删除标识
    /// </summary>
    private bool SoftDelete => throw new NotImplementedException();

    /// <summary>
    ///     操作员托管标识
    /// </summary>
    private bool HandlerHosting => throw new NotImplementedException();

    /// <summary>
    ///     是否启用缓存
    /// </summary>
    protected bool CachedStore => throw new NotImplementedException();

    /// <summary>
    ///     缓存过期时间
    /// </summary>
    private int Expires => throw new NotImplementedException();

    #endregion

    #region SaveChanges

    #region AttacheChange

    /// <summary>
    ///     保存追踪
    /// </summary>
    /// <returns></returns>
    private StoreResult AttacheChange()
    {
        try
        {
            var changes = SaveChanges();
            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    /// <summary>
    ///     保存异步追踪
    /// </summary>
    /// <param name="cancellationToken">取消信号</param>
    /// <returns></returns>
    private async Task<StoreResult> AttacheChangeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var changes = await SaveChangesAsync(cancellationToken);
            return StoreResult.Success(changes);
        }
        catch (DbUpdateConcurrencyException)
        {
            return StoreResult.Failed(Describer.ConcurrencyFailure());
        }
    }

    #endregion

    /// <summary>
    ///     保存当前存储
    /// </summary>
    /// <returns></returns>
    private int SaveChanges()
    {
        if (DebugLogger) Logger?.LogDebug(nameof(SaveChanges));

        return AutoSaveChanges ? Context.SaveChanges() : 0;
    }

    /// <summary>
    ///     保存当前存储
    /// </summary>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns>异步取消结果</returns>
    private Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (DebugLogger) Logger?.LogDebug(nameof(SaveChangesAsync));

        return AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.FromResult(0);
    }

    #endregion

    #region Disposable

    /// <summary>
    ///     已释放标识
    /// </summary>
    private bool _disposed;

    /// <summary>
    ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Throws if this class has been disposed.
    /// </summary>
    /// <exception cref="StoreDisposedException"></exception>
    private void ThrowIfDisposed()
    {
        if (_disposed) throw new StoreDisposedException(GetType());
    }

    #endregion
}