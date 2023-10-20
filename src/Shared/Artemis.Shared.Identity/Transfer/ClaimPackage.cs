using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
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
    string GenerateCheckStamp => Hash.Md5Hash(GenerateFlag);

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
[DataContract]
public record ClaimPackage : IClaim
{
    /// <summary>
    ///     凭据类型
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public virtual required string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public virtual required string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public virtual required string CheckStamp { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    [Required]
    [DataMember(Order = 4)]
    public virtual string? Description { get; set; }

    #region DefaultImplement

    /// <summary>
    ///     生成校验戳
    /// </summary>
    /// <returns></returns>
    public string GenerateCheckStamp => Hash.Md5Hash(GenerateFlag);

    /// <summary>
    ///     生成标识
    /// </summary>
    public string GenerateFlag => $"{ClaimType}:{ClaimValue}";

    #endregion
}

/// <summary>
///     凭据项目实现
/// </summary>
[DataContract]
public sealed record ClaimInfo : ClaimPackage, IKeySlot
{
    /// <summary>
    ///     凭据标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required Guid Id { get; set; }

    /// <summary>
    ///     凭据类型
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 2)]
    public override required string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    [Required]
    [MaxLength(128)]
    [DataMember(Order = 3)]
    public override required string ClaimValue { get; set; }

    /// <summary>
    ///     校验戳
    /// </summary>
    [Required]
    [MaxLength(64)]
    [DataMember(Order = 4)]
    public override required string CheckStamp { get; set; }

    /// <summary>
    ///     描述
    /// </summary>
    [Required]
    [MaxLength(128)]
    [DataMember(Order = 5)]
    public override string? Description { get; set; }
}