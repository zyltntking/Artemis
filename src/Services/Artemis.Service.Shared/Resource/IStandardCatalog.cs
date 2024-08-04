using Artemis.Data.Core;

namespace Artemis.Service.Shared.Resource;

/// <summary>
///     标准目录接口
/// </summary>
public interface IStandardCatalog : IStandardCatalogInfo
{
}

/// <summary>
///     标准目录信息接口
/// </summary>
public interface IStandardCatalogInfo : IKeySlot, IStandardCatalogPackage;

/// <summary>
///     标准目录数据包接口
/// </summary>
public interface IStandardCatalogPackage
{
    /// <summary>
    ///     标准目录名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     标准目录编码
    /// </summary>
    string? Code { get; set; }

    /// <summary>
    ///     标准目录类型
    /// </summary>
    string? Type { get; set; }

    /// <summary>
    ///     标准目录状态
    /// </summary>
    string? State { get; set; }

    /// <summary>
    ///     是否生效
    /// </summary>
    bool Valid { get; set; }

    /// <summary>
    ///     标准目录描述
    /// </summary>
    string? Description { get; set; }
}