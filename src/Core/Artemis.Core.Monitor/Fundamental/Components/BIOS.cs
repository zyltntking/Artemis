using Artemis.Core.Monitor.Fundamental.Components.Interface;
using Artemis.Data.Core.Fundamental.Model;

namespace Artemis.Core.Monitor.Fundamental.Components;

/// <summary>
///   BIOS信息
/// </summary>
/// <remarks><![CDATA[https://learn.microsoft.com/zh-cn/windows/win32/cimwin32prov/win32-bios]]></remarks>
/// <remarks><![CDATA[WMI class: Win32_BIOS]]></remarks>
public class BIOS : IBIOS
{
    #region Implementation of IBIOS

    /// <summary>
    /// 对象说明
    /// </summary>
    public string Caption { get; set; } = string.Empty;

    /// <summary>
    /// 对象描述
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    ///  制造商
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Windows BIOS日期 (UTC) YYYYMMDDHHMMSS.MMMMMM(+-)OOO
    /// </summary>
    public string ReleaseDate { get; set; } = string.Empty;

    /// <summary>
    /// 序列号
    /// </summary>
    public string SerialNumber { get; set; } = string.Empty;

    /// <summary>
    /// 元素标识符
    /// </summary>
    public string SoftwareElementId { get; set; } = string.Empty;

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; } = string.Empty;

    #endregion

    /// <summary>
    ///     元数据信息
    /// </summary>
    public ICollection<MetadataInfo>? Metadata { get; set; }

    #region Overrides of Object

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return
            $"{nameof(Caption)}: {Caption}" + Environment.NewLine +
            $"{nameof(Description)}: {Description}" + Environment.NewLine +
            $"{nameof(Manufacturer)}: {Manufacturer}" + Environment.NewLine +
            $"{nameof(Name)}: {Name}" + Environment.NewLine +
            $"{nameof(ReleaseDate)}: {ReleaseDate}" + Environment.NewLine +
            $"{nameof(SerialNumber)}: {SerialNumber}" + Environment.NewLine +
            $"{nameof(SoftwareElementId)}: {SoftwareElementId}" + Environment.NewLine +
            $"{nameof(Version)}: {Version}";
    }

    #endregion
}