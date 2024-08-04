using Artemis.Data.Core;

namespace Artemis.Service.Shared.Resource;

/// <summary>
/// 标准项目接口
/// </summary>
public interface IStandardItem : IStandardItemInfo
{
}

/// <summary>
/// 标准项目信息接口
/// </summary>
public interface IStandardItemInfo : IStandardItemPackage, IKeySlot
{
    /// <summary>
    /// 标准目录标识
    /// </summary>
    Guid StandardCatalogId { get; set; }
}

/// <summary>
/// 标准项目数据包接口
/// </summary>
public interface IStandardItemPackage
{
    /// <summary>
    /// 标准项目名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// 标准项目编码
    /// </summary>
    string? Code { get; set; }

    /// <summary>
    /// 标准项目最小值
    /// </summary>
    string Minimum { get; set; }

    /// <summary>
    /// 标准项目最大值
    /// </summary>
    string Maximum { get; set; }

    /// <summary>
    /// 标准项目模板
    /// </summary>
    string? Template { get; set; } 

    /// <summary>
    /// 标准项目描述
    /// </summary>
    string? Description { get; set; }
}