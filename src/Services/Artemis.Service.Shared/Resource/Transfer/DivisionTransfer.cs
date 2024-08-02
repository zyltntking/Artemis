using Artemis.Data.Core;

namespace Artemis.Service.Shared.Resource.Transfer;

/// <summary>
///     行政区划树
/// </summary>
public record DivisionInfoTree : DivisionInfo, ITreeInfoSlot<DivisionInfoTree>
{
    /// <summary>
    ///     子节点
    /// </summary>
    public ICollection<DivisionInfoTree>? Children { get; set; }
}

/// <summary>
///     行政区划信息
/// </summary>
public record DivisionInfo : DivisionPackage, IDivisionInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public Guid Id { get; set; }


    /// <summary>
    ///     父标识
    /// </summary>
    public Guid? ParentId { get; set; }
}

/// <summary>
///     行政区划数据包
/// </summary>
public record DivisionPackage : IDivisionPackage
{
    /// <summary>
    ///     行政区划名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     行政区划代码
    /// </summary>
    public required string Code { get; set; }

    /// <summary>
    ///     行政区划级别
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    ///     行政区划类型
    /// </summary>
    public required string Type { get; set; }

    /// <summary>
    ///     行政区划全名
    /// </summary>
    public string? FullName { get; set; }

    /// <summary>
    ///     行政区划拼音
    /// </summary>
    public string? Pinyin { get; set; }

    /// <summary>
    ///     行政区划备注
    /// </summary>
    public string? Remark { get; set; }
}