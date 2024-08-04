namespace Artemis.Service.Shared.Resource.Transfer;

/// <summary>
/// 标准项目信息
/// </summary>
public record StandardItemInfo : StandardItemPackage, IStandardItemInfo
{
    /// <summary>
    /// 标准项目标识
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 标准目录标识
    /// </summary>
    public Guid StandardCatalogId { get; set; }
}

/// <summary>
/// 标准项目数据包
/// </summary>
public record StandardItemPackage : IStandardItemPackage
{

    /// <summary>
    /// 标准项目名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 标准项目编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 标准项目最小值
    /// </summary>
    public required string Minimum { get; set; }

    /// <summary>
    /// 标准项目最大值
    /// </summary>
    public required string Maximum { get; set; }

    /// <summary>
    /// 标准项目模板
    /// </summary>
    public string? Template { get; set; }

    /// <summary>
    /// 标准项目描述
    /// </summary>
    public string? Description { get; set; }

}