using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;

namespace Artemis.Shared.Identity.Transfer;

#region Interface

/// <summary>
///     凭据项目接口
/// </summary>
internal interface IClaim : ICheckStamp
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
    ///     描述
    /// </summary>
    string? Description { get; set; }

    #region DefaultImplement

    /// <summary>
    ///     生成校验戳
    /// </summary>
    /// <returns></returns>
    string GenerateCheckStamp => Normalize.KeyValuePairStampNormalize(ClaimType, ClaimValue);

    /// <summary>
    ///     生成标识
    /// </summary>
    string GenerateFlag => $"{ClaimType}:{ClaimValue}";

    #endregion
}

#endregion

/// <summary>
///     凭据项目实现
/// </summary>
public record ClaimPackage : IClaim
{
    /// <summary>
    ///     凭据类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    public required string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    [Required]
    [MaxLength(128)]
    public required string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    [Required]
    [MaxLength(64)]
    public required string CheckStamp { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    [Required]
    [MaxLength(128)]
    public string? Description { get; set; }

    #region DefaultImplement

    /// <summary>
    ///     生成校验戳
    /// </summary>
    /// <returns></returns>
    public string GenerateCheckStamp => Normalize.KeyValuePairStampNormalize(ClaimType, ClaimValue);

    /// <summary>
    ///     生成标识
    /// </summary>
    public string GenerateFlag => $"{ClaimType}:{ClaimValue}";

    #endregion
}

/// <summary>
///     凭据项目实现
/// </summary>
public sealed record ClaimInfo : ClaimPackage, IKeySlot
{
    /// <summary>
    ///     凭据标识
    /// </summary>
    [Required]
    public required Guid Id { get; set; }
}