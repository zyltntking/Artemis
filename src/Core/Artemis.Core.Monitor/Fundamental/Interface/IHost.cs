using Artemis.Core.Monitor.Fundamental.Types;

namespace Artemis.Core.Monitor.Fundamental.Interface;

/// <summary>
///     主机信息接口
/// </summary>
public interface IHost
{
    /// <summary>
    ///     主机名
    /// </summary>
    string? HostName { get; set; }

    /// <summary>
    ///     主机类型
    /// </summary>
    HostType HostType { get; set; }

    /// <summary>
    /// 实例类型
    /// </summary>
    InstanceType InstanceType { get; set; }

    /// <summary>
    ///     系统名
    /// </summary>
    string? OsName { get; set; }

    /// <summary>
    ///    平台类型
    /// </summary>
    PlatformType PlatformType { get; set; }

    /// <summary>
    ///     系统版本
    /// </summary>
    string? OsVersion { get; set; }

    /// <summary>
    /// 进程架构
    /// </summary>
    string? ProcessArchitecture { get; set; }
}