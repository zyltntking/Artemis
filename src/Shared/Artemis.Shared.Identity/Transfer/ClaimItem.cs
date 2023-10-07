using Artemis.Data.Core;

namespace Artemis.Shared.Identity.Transfer;

/// <summary>
/// 凭据项目接口
/// </summary>
public interface IClaimItem
{
    #region DefaultImplement

    /// <summary>
    ///     生成校验戳
    /// </summary>
    /// <returns></returns>
    string GenerateCheckStamp => Hash.Md5Hash($"{ClaimType}:{ClaimValue}");

    /// <summary>
    ///     生成标识
    /// </summary>
    string GenerateFlag => $"{ClaimType}:{ClaimValue}";

    #endregion

    /// <summary>
    ///     凭据类型
    /// </summary>
    string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    string ClaimValue { get; set; }
}

/// <summary>
/// 凭据项目实现
/// </summary>
public record ClaimItem : IClaimItem
{
    /// <summary>
    ///     生成校验戳
    /// </summary>
    /// <returns></returns>
    public string GenerateCheckStamp => Hash.Md5Hash($"{ClaimType}:{ClaimValue}");

    /// <summary>
    ///     生成标识
    /// </summary>
    public string GenerateFlag => $"{ClaimType}:{ClaimValue}";

    /// <summary>
    ///     凭据类型
    /// </summary>
    public virtual required string ClaimType { get; set; }

    /// <summary>
    ///     凭据值
    /// </summary>
    public virtual required string ClaimValue { get; set; }
}