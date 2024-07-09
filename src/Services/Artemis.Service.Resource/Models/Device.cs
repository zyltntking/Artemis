using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Data.Shared.Resource;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Models;

/// <summary>
///     设备模型
/// </summary>
public class Device : ConcurrencyModel, IDevice
{
    #region Implementation of IDevice

    /// <summary>
    ///     设备名称
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("设备名称")]
    public required string Name { get; set; }

    /// <summary>
    ///     设备类型
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("设备类型")]
    public required string Type { get; set; }

    /// <summary>
    ///     设备代码
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("设备代码")]
    public required string Code { get; set; }

    /// <summary>
    ///     设备型号
    /// </summary>
    [Required]
    [MaxLength(128)]
    [Comment("设备型号")]
    public required string Model { get; set; }

    /// <summary>
    ///     设备描述
    /// </summary>
    [MaxLength(256)]
    [Comment("设备描述")]
    public string? Description { get; set; }

    #endregion
}