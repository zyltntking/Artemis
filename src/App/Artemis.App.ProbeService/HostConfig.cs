using Artemis.Core.Monitor.Fundamental.Types;

namespace Artemis.App.ProbeService;

/// <summary>
///     主机配置
/// </summary>
public class HostConfig
{
    /// <summary>
    ///     主机类型
    /// </summary>
    public HostType HostType { get; set; } = HostType.Unknown;

    /// <summary>
    ///     实例类型
    /// </summary>
    public InstanceType InstanceType { get; set; } = InstanceType.Unknown;
}