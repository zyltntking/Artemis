using Artemis.Core.Monitor.Fundamental.Components.Interface;
using Artemis.Data.Core.Fundamental.Model;

namespace Artemis.Core.Monitor.Fundamental.Components;

/// <summary>
///     操作系统信息
/// </summary>
public class OS : IOS
{
    #region Overrides of Object

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString()
    {
        return
            $"Name: {Name}{Environment.NewLine}Version: {Version}{Environment.NewLine}";
    }

    #endregion

    #region Implementation of IOS

    /// <summary>
    ///     操作系统名
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     操作系统版本
    /// </summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>
    ///     元数据信息
    /// </summary>
    public ICollection<MetadataInfo>? Metadata { get; set; }

    #endregion
}