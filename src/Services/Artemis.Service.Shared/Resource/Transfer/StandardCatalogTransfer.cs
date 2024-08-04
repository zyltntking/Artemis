namespace Artemis.Service.Shared.Resource.Transfer;

/// <summary>
/// 标准目录信息
/// </summary>
public record StandardCatalogInfo : StandardCatalogPackage, IStandardCatalogInfo
{
    /// <summary>
    /// 存储标识
    /// </summary>
    public Guid Id { get; set; }
}

/// <summary>
/// 标准目录数据包
/// </summary>
public record StandardCatalogPackage : IStandardCatalogPackage
{

    /// <summary>
    /// 标准目录名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 标准目录编码
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    /// 标准目录类型
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// 标准目录状态
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// 是否生效
    /// </summary>
    public bool Valid { get; set; }

    /// <summary>
    /// 标准目录描述
    /// </summary>
    public string? Description { get; set; }

}
