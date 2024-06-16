namespace Artemis.Data.Shared.Identity;

/// <summary>
/// 角色接口
/// </summary>
public interface IRole : IRoleInfo
{
    /// <summary>
    ///     标准化角色名
    /// </summary>
    string NormalizedName { get; set; }
}

/// <summary>
/// 角色信息接口
/// </summary>
public interface IRoleInfo
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