namespace Artemis.Data.Store;

#region Interface

/// <summary>
///     具键存储管理器配置接口
/// </summary>
public interface IManagerOptions
{
    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    bool EnableLogger { get; set; }
}

#endregion

/// <summary>
///     具键存储管理器配置实例
/// </summary>
public class ManagerOptions : IManagerOptions
{
    #region Implementation of IManagerOptions

    /// <summary>
    ///     是否启用Debug日志
    /// </summary>
    public bool EnableLogger { get; set; } = true;

    #endregion
}