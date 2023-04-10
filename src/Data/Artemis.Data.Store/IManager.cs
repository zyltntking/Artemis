using Artemis.Data.Core;

namespace Artemis.Data.Store;

/// <summary>
///     提供用于管理TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
public interface IManager<TEntity> : IManager<TEntity, Guid> where TEntity : IModelBase
{
}

/// <summary>
///     提供用于管理TEntity的存储器的API
/// </summary>
/// <typeparam name="TEntity">实体类型</typeparam>
/// <typeparam name="TKey">键类型</typeparam>
public interface IManager<TEntity, TKey> where TEntity : IModelBase<TKey> where TKey : IEquatable<TKey>
{
}