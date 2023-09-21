namespace Artemis.Shared.Identity.Transfer.Interface;

/// <summary>
///     基本角色接口
/// </summary>
public interface IRole
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