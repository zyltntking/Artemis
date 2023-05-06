namespace Artemis.Core.Monitor.Fundamental.Components.Interface;

/// <summary>
///  BIOS信息接口
/// </summary>
/// <remarks><![CDATA[https://learn.microsoft.com/zh-cn/windows/win32/cimwin32prov/win32-bios]]></remarks>
/// <remarks><![CDATA[WMI class: Win32_BIOS]]></remarks>
public interface IBIOS
{
    /// <summary>
    /// 对象说明
    /// </summary>
    public string Caption { get; set; }

    /// <summary>
    /// 对象描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///  制造商
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Windows BIOS日期 (UTC) YYYYMMDDHHMMSS.MMMMMM(+-)OOO
    /// </summary>
    public string ReleaseDate { get; set; }

    /// <summary>
    /// 序列号
    /// </summary>
    public string SerialNumber { get; set; }

    /// <summary>
    /// 元素标识符
    /// </summary>
    public string SoftwareElementId { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    public string Version { get; set; }
}