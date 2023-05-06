using Artemis.Data.Core.Fundamental.Model;

namespace Artemis.Core.Monitor.Fundamental.Components.Interface;

/// <summary>
///     操作系统信息接口
/// </summary>
/// <remarks><![CDATA[https://learn.microsoft.com/zh-cn/windows/win32/cimwin32prov/win32-operatingsystem]]></remarks>
/// <remarks><![CDATA[WMI class: Win32_OperatingSystem]]></remarks>
public interface IOS
{
    /// <summary>
    ///     操作系统名
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     操作系统版本
    /// </summary>
    string Version { get; set; }
}