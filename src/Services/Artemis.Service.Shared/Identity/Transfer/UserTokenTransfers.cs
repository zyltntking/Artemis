namespace Artemis.Service.Shared.Identity.Transfer;

/// <summary>
///     用户令牌信息
/// </summary>
public record UserTokenInfo : UserTokenPackage, IUserTokenInfo
{
    /// <summary>
    ///     用户标识
    /// </summary>
    public Guid UserId { get; set; }
}

/// <summary>
///     用户令牌数据包接口
/// </summary>
public record UserTokenPackage : IUserTokenPackage
{
    #region Implementation of IUserTokenPackage

    /// <summary>
    ///     登录提供程序
    /// </summary>
    public required string LoginProvider { get; set; }

    /// <summary>
    ///     令牌名称
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    ///     令牌值
    /// </summary>
    public string? Value { get; set; }

    #endregion
}