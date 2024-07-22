namespace Artemis.Service.Shared.Identity.Transfer;

/// <summary>
///     凭据信息
/// </summary>
public record ClaimInfo : ClaimPackage, IClaimInfo
{
    /// <summary>
    ///     存储标识
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    public required string CheckStamp { get; set; }

}

/// <summary>
/// 凭据数据包
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