namespace Artemis.Data.Store.Configuration;

/// <summary>
///     实体约定
/// </summary>
public static class EntityConvention
{
    /// <summary>
    ///     键名称约定
    /// </summary>
    /// <param name="dataSetName">数据集名称</param>
    /// <returns></returns>
    public static string KeyName(this string dataSetName)
    {
        return $"PK_{dataSetName}";
    }

    /// <summary>
    ///     索引名称约定
    /// </summary>
    /// <param name="dataSetName"></param>
    /// <param name="columnNames"></param>
    /// <returns></returns>
    public static string IndexName(this string dataSetName, params string[] columnNames)
    {
        return $"IX_{dataSetName}_{string.Join("_", columnNames)}";
    }

    /// <summary>
    ///     外键名称约定
    /// </summary>
    /// <param name="dataSetName">数据集名称</param>
    /// <param name="referenceDataSetName">引用数据集名称</param>
    /// <returns></returns>
    public static string ForeignKeyName(this string dataSetName, string referenceDataSetName)
    {
        return $"FK_{dataSetName}_{referenceDataSetName}";
    }
}