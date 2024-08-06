using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Service.Shared.Resource;

/// <summary>
/// 系统模块
/// </summary>
public interface ISystemModule : ISystemModuleInfo;

/// <summary>
/// 系统模块信息接口
/// </summary>
public interface ISystemModuleInfo : ISystemModulePackage, IKeySlot, IParentKeySlot;

/// <summary>
/// 系统模块数据包接口
/// </summary>
public interface ISystemModulePackage
{
    /// <summary>
    /// 模块名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// 模块类型
    /// </summary>
    SystemModuleType Type { get; set; }

    /// <summary>
    /// 模块序列
    /// </summary>
    int Order { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    string? Path { get; set; }

    /// <summary>
    /// 路由参数
    /// </summary>
    string? Parameters { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    string? Component { get; set; }

    /// <summary>
    /// 模块图标
    /// </summary>
    string? Icon { get; set; }

    /// <summary>
    /// 是否外链
    /// </summary>
    bool IsFrame { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// 模块状态
    /// </summary>
    bool Status { get; set; }

    /// <summary>
    /// 凭据戳
    /// </summary>
    string? ClaimStamp { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    string? Remark { get; set; }
}