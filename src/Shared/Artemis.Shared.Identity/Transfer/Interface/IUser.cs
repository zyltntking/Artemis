namespace Artemis.Shared.Identity.Transfer.Interface;

/// <summary>
///     基本用户接口
/// </summary>
public interface IUser
{
    /// <summary>
    ///     用户名
    /// </summary>
    string UserName { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    string? Email { get; set; }

    /// <summary>
    ///     电子邮件确认戳
    /// </summary>
    bool EmailConfirmed { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    string? PhoneNumber { get; set; }

    /// <summary>
    ///     电话号码确认戳
    /// </summary>
    bool PhoneNumberConfirmed { get; set; }
}