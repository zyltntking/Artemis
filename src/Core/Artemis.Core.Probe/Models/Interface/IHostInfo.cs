using Artemis.Core.Probe.Types;

namespace Artemis.Core.Probe.Models.Interface;

/// <summary>
///     主机信息接口
/// </summary>
public interface IHostInfo
{
    /// <summary>
    ///     主机名
    /// </summary>
    string HostName { get; set; }

    /// <summary>
    ///     主机类型
    /// </summary>
    HostType HostType { get; set; }

    /// <summary>
    ///     系统名
    /// </summary>
    string OsName { get; set; }

    /// <summary>
    ///     系统版本
    /// </summary>
    string OsVersion { get; set; }
}