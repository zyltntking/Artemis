using Artemis.Core.Monitor.Fundamental.Interface;

namespace Artemis.Core.Monitor;

/// <summary>
///     探针接口
/// </summary>
public interface IProbe
{
    /// <summary>
    ///     主机信息接口
    /// </summary>
    IHostInfo HostInfo { get; }
}