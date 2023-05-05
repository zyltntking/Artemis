namespace Artemis.Data.Store;

/// <summary>
///     数据类型适配器
/// </summary>
public static class DataTypeAdapter
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
                        DateTime = "DATE",
                        Boolean = "NUMBER(1)",
                        Integer = "INTEGER",
                        Long = "INTEGER",
                        Double = "NUMBER(18, 4)",
                        StringTemplate = "NVARCHAR2({0})"
                    }
                },
                {
                    DbType.SqlServer, new DataTypeSet
                    {
                        DateTime = "DATETIME",
                        Boolean = "BIT",
                        Integer = "INTEGER",
                        Long = "INTEGER",
                        Double = "FLOAT",
                        StringTemplate = "NVARCHAR({0})"
                    }
                },
                {
                    DbType.MySql, new DataTypeSet
                    {
                        DateTime = "DATETIME",
                        Boolean = "BIT",
                        Integer = "INTEGER",
                        Long = "INTEGER",
                        Double = "FLOAT",
                        StringTemplate = "NVARCHAR({0})"
                    }
                },
                {
                    DbType.PostgreSql, new DataTypeSet
                    {
                        DateTime = "TIMESTAMP",
                        Boolean = "BOOLEAN",
                        Integer = "INTEGER",
                        Long = "INTEGER",
                        Double = "DOUBLE PRECISION",
                        StringTemplate = "NVARCHAR({0})"
                    }
                },
                {
                    DbType.Sqlite, new DataTypeSet
                    {
                        DateTime = "DATETIME",
                        Boolean = "BOOLEAN",
                        Integer = "INTEGER",
                        Long = "INTEGER",
                        Double = "FLOAT",
                        StringTemplate = "NVARCHAR({0})"
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
public class DataTypeSet
{
    /// <summary>
    ///     DateTime数据类型
    /// </summary>
    public string DateTime { get; init; } = "DATETIME";

    /// <summary>
    ///     Boolean数据类型
    /// </summary>
    public string Boolean { get; init; } = "BIT";

    /// <summary>
    ///     Integer数据类型
    /// </summary>
    public string Integer { get; init; } = "INTEGER";

    /// <summary>
    ///     Long数据类型
    /// </summary>
    public string Long { get; init; } = "INTEGER";

    /// <summary>
    ///     Double数据类型
    /// </summary>
    public string Double { get; init; } = "FLOAT";

    /// <summary>
    ///     String数据类型
    /// </summary>
    public string StringTemplate { get; init; } = "VARCHAR({0})";

    /// <summary>
    ///     获取String数据类型
    /// </summary>
    /// <param name="length">字段长度</param>
    /// <returns></returns>
    public string String(int length)
    {
        return string.Format(StringTemplate, length);
    }
}