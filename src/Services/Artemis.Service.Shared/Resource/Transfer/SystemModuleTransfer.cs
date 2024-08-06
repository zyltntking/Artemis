using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Service.Shared.Resource.Transfer;

/// <summary>
/// 系统模块树
/// </summary>
public record SystemModuleInfoTree : SystemModuleInfo, ITreeInfoSlot<SystemModuleInfoTree>
{
    /// <summary>
    ///     子节点
    /// </summary>
    public ICollection<SystemModuleInfoTree>? Children { get; set; }
}

/// <summary>
/// 系统模块信息
/// </summary>
public record SystemModuleInfo : SystemModulePackage, ISystemModuleInfo
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
/// 系统模块数据包
/// </summary>
public record SystemModulePackage : ISystemModulePackage
{
    #region Implementation of ISystemModulePackage

    /// <summary>
    /// 模块名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// 模块类型
    /// </summary>
    public required SystemModuleType Type { get; set; }

    /// <summary>
    /// 模块序列
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 路由参数
    /// </summary>
    public string? Parameters { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    public string? Component { get; set; }

    /// <summary>
    /// 模块图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 是否外链
    /// </summary>
    public bool IsFrame { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    public bool Visible { get; set; }

    /// <summary>
    /// 模块状态
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// 凭据戳
    /// </summary>
    public string? ClaimStamp { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    #endregion
} 