namespace Artemis.Data.Shared.Identity;

/// <summary>
///     凭据接口
/// </summary>
public interface IClaim
{
    /// <summary>
    ///     凭据类型
    /// </summary>
    string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    string CheckStamp { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    string? Description { get; set; }
}