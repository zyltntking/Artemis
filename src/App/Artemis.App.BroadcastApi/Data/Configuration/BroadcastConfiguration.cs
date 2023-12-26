using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Configuration;

namespace Artemis.App.BroadcastApi.Data.Configuration;

/// <summary>
///     广播模型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class BroadcastConfiguration<TEntity> : MateSlotModelConfiguration<TEntity>
    where TEntity : class, IMateSlot
{
    #region Overrides of ModelBaseConfiguration<TEntity>

    /// <summary>
    ///     数据库类型
    /// </summary>
    protected override DbType DbType => DbType.PostgreSql;

    #endregion
}