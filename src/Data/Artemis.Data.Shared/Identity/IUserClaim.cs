namespace Artemis.Data.Shared.Identity;

/// <summary>
///  用户凭据接口
/// </summary>
public interface IUserClaim : IClaim
{
    /// <summary>
    ///     用户标识
    /// </summary>
    Guid UserId { get; set; }
}