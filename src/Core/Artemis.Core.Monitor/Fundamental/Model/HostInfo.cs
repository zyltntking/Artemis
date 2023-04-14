using Artemis.Core.Monitor.Fundamental.Interface;
using Artemis.Core.Monitor.Fundamental.Types;

namespace Artemis.Core.Monitor.Fundamental.Model;

/// <summary>
/// 主机信息
/// </summary>
public class HostInfo : IHost
{
    #region Implementation of IHost

    /// <summary>
    ///     主机名
    /// </summary>
    public string HostName { get; set; }

    /// <summary>
    ///     主机类型
    /// </summary>
    public HostType HostType { get; set; }

    /// <summary>
    ///     系统名
    /// </summary>
    public string OsName { get; set; }

    /// <summary>
    ///     系统版本
    /// </summary>
    public string OsVersion { get; set; }

    #endregion
}