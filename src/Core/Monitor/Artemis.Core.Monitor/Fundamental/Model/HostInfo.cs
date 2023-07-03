using Artemis.Core.Monitor.Fundamental.Interface;
using Artemis.Core.Monitor.Fundamental.Types;

namespace Artemis.Core.Monitor.Fundamental.Model;

/// <summary>
///     主机信息
/// </summary>
public class HostInfo : IHostInfo
{
    #region Implementation of IHost

    /// <summary>
    ///     主机名
    /// </summary>
    public string? HostName { get; set; }

    /// <summary>
    ///     主机类型
    /// </summary>
    public HostType HostType { get; set; } = HostType.Unknown;

    /// <summary>
    ///     实例类型
    /// </summary>
    public InstanceType InstanceType { get; set; } = InstanceType.Unknown;

    /// <summary>
    ///     系统名
    /// </summary>
    public string? OsName { get; set; }

    /// <summary>
    ///     平台类型
    /// </summary>
    public PlatformType PlatformType { get; set; } = PlatformType.Unknown;

    /// <summary>
    ///     系统版本
    /// </summary>
    public string? OsVersion { get; set; }

    /// <summary>
    ///     进程架构
    /// </summary>
    public string? OsArchitecture { get; set; }

    #endregion
}