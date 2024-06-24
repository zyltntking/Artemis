namespace Artemis.Data.Shared.Identity;

/// <summary>
///     基本用户令牌信息接口
/// </summary>
public interface IUserToken : IUserTokenInfo;

/// <summary>
///     用户令牌信息接口
/// </summary>
public interface IUserTokenInfo : IUserTokenPackage
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }
}

/// <summary>
///     基本用户令牌数据包接口
/// </summary>
public interface IUserTokenPackage
{
    /// <summary>
    ///     登录提供程序
    /// </summary>
    string LoginProvider { get; set; }

    /// <summary>
    ///     令牌名称
    /// </summary>
    string Name { get; set; }

    /// <summary>
    ///     令牌值
    /// </summary>
    string? Value { get; set; }
}