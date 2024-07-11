using Artemis.Data.Core;

namespace Artemis.Service.Shared.Identity;

/// <summary>
///     凭据接口
/// </summary>
public interface IClaim : IClaimDocument;

/// <summary>
///     凭据信息
/// </summary>
public interface IClaimInfo : IClaimDocument, IKeySlot
{
}

/// <summary>
///     凭据文档接口
/// </summary>
public interface IClaimDocument : IClaimPackage, ICheckStamp;

/// <summary>
///     凭据数据包接口
/// </summary>
public interface IClaimPackage
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