using Artemis.Data.Core;
using Artemis.Data.Store;

namespace Artemis.Core.Monitor.Store.Entities.Configurations;

/// <summary>
/// 监控配置
/// </summary>
/// <typeparam name="TEntity">类型模板</typeparam>
public class MonitorConfiguration<TEntity> : PartitionBaseTypeConfiguration<TEntity> where TEntity : class, IPartitionBase
{
    /// <summary>
    ///    数据库类型
    /// </summary>
    /// <remarks>修改配置来源</remarks>
    protected override DbType DbType => MonitorSetting.DbType;
}