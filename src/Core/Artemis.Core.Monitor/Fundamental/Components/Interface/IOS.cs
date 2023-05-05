using Artemis.Data.Core.Fundamental.Model;

namespace Artemis.Core.Monitor.Fundamental.Components.Interface;

/// <summary>
///     操作系统信息接口
/// </summary>
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

    /// <summary>
    ///     元数据信息
    /// </summary>
    ICollection<MetadataInfo>? Metadata { get; set; }
}