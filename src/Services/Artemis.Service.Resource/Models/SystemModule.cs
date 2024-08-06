using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Resource;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Models;

/// <summary>
/// 系统模块
/// </summary>
public class SystemModule : ConcurrencyModel, ISystemModule
{
    /// <summary>
    /// 模块名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    public required string Name { get; set; }

    /// <summary>
    /// 模块类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [Comment("模块类型")]
    public required string Type { get; set; }

    /// <summary>
    /// 模块序列
    /// </summary>
    [Comment("模块序列")]
    public int Order { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    [MaxLength(128)]
    [Comment("路由地址")]
    public string? Path { get; set; }

    /// <summary>
    /// 路由参数
    /// </summary>
    [MaxLength(256)]
    [Comment("路由参数")]
    public string? Parameters { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    [MaxLength(128)]
    [Comment("组件路径")]
    public string? Component { get; set; }

    /// <summary>
    /// 模块图标
    /// </summary>
    [MaxLength(64)]
    [Comment("模块图标")]
    public string? Icon { get; set; }

    /// <summary>
    /// 是否外链
    /// </summary>
    [Comment("是否外链")]
    public bool IsFrame { get; set; }

    /// <summary>
    /// 是否显示
    /// </summary>
    [Comment("是否显示")]
    public bool Visible { get; set; }

    /// <summary>
    /// 模块状态
    /// </summary>
    [Comment("模块状态")]
    public bool Status { get; set; }

    /// <summary>
    /// 凭据戳
    /// </summary>
    [MaxLength(64)]
    [Comment("凭据戳")]
    public string? ClaimStamp { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [MaxLength(256)]
    [Comment("备注")]
    public string? Remark { get; set; }

    /// <summary>
    ///     上级模块标识
    /// </summary>
    [Comment("上级模块标识")]
    public Guid? ParentId { get; set; }

}