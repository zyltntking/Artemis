using System.ComponentModel;
using Artemis.Data.Core.Fundamental;

namespace Artemis.Extensions.Web.Identity;

/// <summary>
///     认证等级
/// </summary>
[Description("认证等级")]
public sealed class ArtemisIdentityLevel : Enumeration
{
    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private ArtemisIdentityLevel(int id, string name) : base(id, name)
    {
    }

    /// <summary>
    ///     匿名许可
    /// </summary>
    [Description("匿名许可")]
    public static ArtemisIdentityLevel Anonymous { get; } = new(0, nameof(Anonymous));

    /// <summary>
    ///     认证令牌
    /// </summary>
    [Description("认证令牌")]
    public static ArtemisIdentityLevel Token { get; } = new(1, nameof(Token));

    /// <summary>
    ///     认证角色
    /// </summary>
    [Description("认证角色")]
    public static ArtemisIdentityLevel Role { get; } = new(2, nameof(Role));

    /// <summary>
    ///     认证凭据
    /// </summary>
    [Description("认证凭据")]
    public static ArtemisIdentityLevel Claim { get; } = new(3, nameof(Claim));
}

/// <summary>
///     常量
/// </summary>
public static class Constants
{
    /// <summary>
    ///     头Token键
    /// </summary>
    public const string HeaderTokenKey = "Token";

    /// <summary>
    ///     缓存Token前缀
    /// </summary>
    public const string CacheTokenPrefix = "Artemis:Identity:Token";
}