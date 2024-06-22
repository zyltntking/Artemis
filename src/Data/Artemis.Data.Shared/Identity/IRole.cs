namespace Artemis.Data.Shared.Identity;

/// <summary>
///     角色接口
/// </summary>
public interface IRole : IRolePackage
{
    /// <summary>
    ///     标准化角色名
    /// </summary>
    string NormalizedName { get; set; }
}

/// <summary>
///     角色数据包接口
/// </summary>
public interface IRolePackage
{
    /// <summary>
    ///     角色名
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     角色描述
    /// </summary>
    string? Description { get; set; }
}