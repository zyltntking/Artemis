using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;

namespace Artemis.Extensions.Identity;

/// <summary>
///     令牌记录
/// </summary>
public record TokenRecord
{
    /// <summary>
    ///     用户标识
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    ///     用户名
    /// </summary>
    public required string UserName { get; set; }

    /// <summary>
    ///     端类型
    /// </summary>
    public required string EndType { get; set; }

    /// <summary>
    ///     过期时间
    /// </summary>
    public required int Expire { get; set; }

    /// <summary>
    ///     用户凭据
    /// </summary>
    public required IEnumerable<ClaimRecord> UserClaims { get; set; }

    /// <summary>
    ///     角色标识
    /// </summary>
    public required IEnumerable<RoleRecord> Roles { get; set; }

    /// <summary>
    ///     角色凭据
    /// </summary>
    public required IEnumerable<ClaimRecord> RoleClaims { get; set; }
}

/// <summary>
///     令牌记录扩展
/// </summary>
public static class TokenRecordExtensions
{
    /// <summary>
    ///     生成Token符号
    /// </summary>
    /// <param name="record">token记录</param>
    /// <returns></returns>
    public static string TokenSymbol(this TokenRecord record)
    {
        var symbol = Hash.PasswordHash($"{record.UserId}:{record.UserName}");

        return Hash.HashData(symbol, HmacHashType.HmacMd5);
    }
}

/// <summary>
///     签名结果
/// </summary>
public sealed record SignResult
{
    /// <summary>
    ///     是否签名成功
    /// </summary>
    public bool Succeeded { get; set; }

    /// <summary>
    ///     认证消息
    /// </summary>
    public required string Message { get; set; }
}

/// <summary>
///     凭据记录
/// </summary>
public record ClaimRecord
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
}

/// <summary>
///     角色记录
/// </summary>
public record RoleRecord
{
    /// <summary>
    ///     角色标识
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    ///     角色名
    /// </summary>
    public required string Name { get; set; }
}