using Artemis.Data.Store;

namespace Artemis.Core.Monitor.Store;

/// <summary>
/// 全局配置
/// </summary>
public static class Constants
{
    /// <summary>
    /// 数据库类型
    /// </summary>
    public static DbType DbType { get; } = DbType.PostgreSql;
}