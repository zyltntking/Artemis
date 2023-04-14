using Artemis.Data.Core.Fundamental;

namespace Artemis.Data.Store;

/// <summary>
///     数据库类型
/// </summary>
public class DbType : Enumeration
{
    /// <summary>
    ///     Unknown
    /// </summary>
    public static DbType Unknown = new(-1, nameof(Unknown));

    /// <summary>
    ///     Oracle
    /// </summary>
    public static DbType Oracle = new(0, nameof(Oracle));

    /// <summary>
    ///     SqlServer
    /// </summary>
    public static DbType SqlServer = new(1, nameof(SqlServer));

    /// <summary>
    ///     MySql
    /// </summary>
    public static DbType MySql = new(2, nameof(MySql));

    /// <summary>
    ///     PostgreSql
    /// </summary>
    public static DbType PostgreSql = new(3, nameof(PostgreSql));

    /// <summary>
    ///     Sqlite
    /// </summary>
    public static DbType Sqlite = new(4, nameof(Sqlite));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private DbType(int id, string name) : base(id, name)
    {
    }
}