namespace Artemis.Extensions.ServiceConnect.SwaggerFilters.Descriptor;

/// <summary>
///     架构信息接口
/// </summary>
internal interface ISchemaInfo
{
    /// <summary>
    ///     最小值
    /// </summary>
    int? Minimum { get; set; }

    /// <summary>
    ///     最大值
    /// </summary>
    int? Maximum { get; set; }

    /// <summary>
    ///     最短长度
    /// </summary>
    int? MinLength { get; set; }

    /// <summary>
    ///     最长长度
    /// </summary>
    int? MaxLength { get; set; }

    /// <summary>
    ///     格式
    /// </summary>
    string? Format { get; set; }

    /// <summary>
    ///     例子
    /// </summary>
    string? Example { get; set; }

    /// <summary>
    ///     匹配模式
    /// </summary>
    string? Pattern { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    string? Description { get; set; }

    /// <summary>
    ///     是否是密码
    /// </summary>
    bool IsPassword { get; set; }
}

/// <summary>
///     架构信息
/// </summary>
internal sealed class SchemaInfo : ISchemaInfo
{
    #region Implementation of ISchemaInfo

    /// <summary>
    ///     最小值
    /// </summary>
    public int? Minimum { get; set; }

    /// <summary>
    ///     最大值
    /// </summary>
    public int? Maximum { get; set; }

    /// <summary>
    ///     最短长度
    /// </summary>
    public int? MinLength { get; set; }

    /// <summary>
    ///     最长长度
    /// </summary>
    public int? MaxLength { get; set; }

    /// <summary>
    ///     格式
    /// </summary>
    public string? Format { get; set; }

    /// <summary>
    ///     例子
    /// </summary>
    public string? Example { get; set; }

    /// <summary>
    ///     匹配模式
    /// </summary>
    public string? Pattern { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///     是否是密码
    /// </summary>
    public bool IsPassword { get; set; } = false;

    #endregion
}