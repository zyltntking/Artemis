using Artemis.Data.Shared.Identity;

namespace Artemis.Data.Shared.Transfer;

/// <summary>
///     用户信息
/// </summary>
public class UserInfo : IUserInfo
{
    #region Implementation of IUserInfo

    /// <summary>
    ///     用户名
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    public string? PhoneNumber { get; set; }

    #endregion
}