using Artemis.Service.Shared.Identity;

namespace Artemis.Service.Shared.Transfer.Identity;

/// <summary>
///     凭据信息
/// </summary>
public record ClaimInfo : ClaimDocument, IClaimInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public required Guid Id { get; set; }
}

/// <summary>
///     凭据文档
/// </summary>
public record ClaimDocument : ClaimPackage, IClaimDocument
{
    /// <summary>
    ///     校验戳
    /// </summary>
    public required string CheckStamp { get; set; }
}

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
    ///     描述
    /// </summary>
    public string? Description { get; set; }
}