using Artemis.Data.Core;

namespace Artemis.Data.Shared.Identity;

/// <summary>
///     凭据接口
/// </summary>
public interface IClaim : IClaimPackage;

/// <summary>
///     凭据信息
/// </summary>
public interface IClaimInfo : IClaimPackage, IKeySlot
{
}

/// <summary>
///     凭据数据包接口
/// </summary>
public interface IClaimPackage : ICheckStamp
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
}