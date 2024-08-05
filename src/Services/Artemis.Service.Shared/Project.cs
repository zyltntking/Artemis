namespace Artemis.Service.Shared;

/// <summary>
///     项目包装器
/// </summary>
public static class Project
{
    /// <summary>
    ///     默认前缀
    /// </summary>
    private const string DefaultPrefix = "Artemis";

    /// <summary>
    ///     项目前缀
    /// </summary>
    public static string Prefix { get; set; } = "Artemis";

    /// <summary>
    ///     获取配置表名
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public static string TableName(this string tableName)
    {
        if (tableName.StartsWith(DefaultPrefix) && DefaultPrefix != Prefix)
            return tableName.Replace(DefaultPrefix, Prefix);

        return tableName;
    }

    /// <summary>
    ///     模式
    /// </summary>
    public static class Schemas
    {
        /// <summary>
        ///     认证服务模式
        /// </summary>
        public const string Identity = nameof(Identity);

        /// <summary>
        ///     资源服务模式
        /// </summary>
        public const string Resource = nameof(Resource);

        /// <summary>
        ///     原始数据服务模式
        /// </summary>
        public const string Business = nameof(Business);

        /// <summary>
        ///     学校服务模式
        /// </summary>
        public const string School = nameof(School);

        /// <summary>
        ///     任务服务模式
        /// </summary>
        public const string Task = nameof(Task);
    }
}