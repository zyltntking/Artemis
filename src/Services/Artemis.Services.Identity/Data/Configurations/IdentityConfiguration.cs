using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Configuration;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     Artemis认证模型配置
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public abstract class IdentityConfiguration<TEntity> : ArtemisMateSlotConfiguration<TEntity>
    where TEntity : class, IMateSlot
{
    #region Overrides of ArtemisConfiguration<TEntity>

    /// <summary>
    ///     数据库类型
    /// </summary>
    protected override DbType DbType => DbType.PostgreSql;

    #endregion
}