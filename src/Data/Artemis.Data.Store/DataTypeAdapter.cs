namespace Artemis.Data.Store;

/// <summary>
///     数据类型适配器
/// </summary>
internal static class DataTypeAdapter
{
    /// <summary>
    ///     数据类型缓存
    /// </summary>
    private static Dictionary<DbType, DataTypeSet>? _dataTypeDictionary;

    /// <summary>
    ///     数据类型访问器
    /// </summary>
    private static Dictionary<DbType, DataTypeSet> DataTypeDictionary
    {
        get
        {
            return _dataTypeDictionary ??= new Dictionary<DbType, DataTypeSet>
            {
                {
                    DbType.Oracle, new DataTypeSet
                    {
                        Guid = "CHAR(36)",
                        DateTime = "DATE",
                        Boolean = "NUMBER(1)",
                        Integer = "INTEGER",
                        Long = "INTEGER",
                        Double = "FLOAT"
                    }
                },
                {
                    DbType.SqlServer, new DataTypeSet
                    {
                        Guid = "UNIQUEIDENTIFIER",
                        DateTime = "DATETIME",
                        Boolean = "BIT",
                        Integer = "INT",
                        Long = "BIGINT",
                        Double = "FLOAT"
                    }
                },
                {
                    DbType.MySql, new DataTypeSet
                    {
                        Guid = "CHAR(36)",
                        DateTime = "DATETIME",
                        Boolean = "BIT",
                        Integer = "INT",
                        Long = "BIGINT",
                        Double = "FLOAT"
                    }
                },
                {
                    DbType.PostgreSql, new DataTypeSet
                    {
                        Guid = "UUID",
                        DateTime = "TIMESTAMP",
                        Boolean = "BOOLEAN",
                        Integer = "INTEGER",
                        Long = "BIGINT",
                        Double = "DOUBLE PRECISION"
                    }
                },
                {
                    DbType.Sqlite, new DataTypeSet
                    {
                        Guid = "CHAR(36)",
                        DateTime = "DATETIME",
                        Boolean = "BOOLEAN",
                        Integer = "INTEGER",
                        Long = "INTEGER",
                        Double = "FLOAT"
                    }
                }
            };
        }
    }

    /// <summary>
    ///     获取数据类型集
    /// </summary>
    /// <param name="dbType">数据库类型</param>
    /// <returns>数据类型集</returns>
    public static DataTypeSet GetDataTypeSet(DbType dbType)
    {
        return DataTypeDictionary[dbType];
    }
}

/// <summary>
///     数据类型集
/// </summary>
public record DataTypeSet
{
    /// <summary>
    ///     Guid数据类型
    /// </summary>
    public required string Guid { get; init; } = "UNIQUEIDENTIFIER";

    /// <summary>
    ///     DateTime数据类型
    /// </summary>
    public required string DateTime { get; init; } = "DATETIME";

    /// <summary>
    ///     Boolean数据类型
    /// </summary>
    public required string Boolean { get; init; } = "BIT";

    /// <summary>
    ///     Integer数据类型
    /// </summary>
    public required string Integer { get; init; } = "INT";

    /// <summary>
    ///     Long数据类型
    /// </summary>
    public required string Long { get; init; } = "BIGINT";

    /// <summary>
    ///     Double数据类型
    /// </summary>
    public required string Double { get; init; } = "FLOAT";
}