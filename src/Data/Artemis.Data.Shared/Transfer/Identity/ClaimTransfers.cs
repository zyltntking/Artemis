using Artemis.Data.Shared.Identity;

namespace Artemis.Data.Shared.Transfer.Identity;

/// <summary>
///     凭据项目实现
/// </summary>
public record ClaimPackage : IClaimPackage
{
    /// <summary>
    ///     凭据类型
    /// </summary>
    public required string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    public required string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    public required string CheckStamp { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    public string? Description { get; set; }
}