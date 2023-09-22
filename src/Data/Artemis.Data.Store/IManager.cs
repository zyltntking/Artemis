using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
///     提供用于管理TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IManager<TEntity> : IManager<TEntity, Guid> where TEntity : IModelBase
{
}

/// <summary>
///     提供用于管理TEntity的存储器的API接口
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IManager<TEntity, TKey> where TEntity : IModelBase<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    ///     规范化键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns>规范化后的键</returns>
    public string NormalizeKey(string key);

    /// <summary>
    ///     缓存键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    public void SetKey(string key, TKey value);

    /// <summary>
    ///     缓存键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="value">值</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task? SetKeyAsync(string key, TKey value, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取键
    /// </summary>
    /// <param name="key">键</param>
    /// <returns></returns>
    public TKey? GetKey(string key);

    /// <summary>
    /// 获取键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task<TKey?> GetKeyAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// 移除键
    /// </summary>
    /// <param name="key">键</param>
    public void RemoveKey(string key);

    /// <summary>
    /// 移除键
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public Task? RemoveKeyAsync(string key, CancellationToken cancellationToken = default);
}