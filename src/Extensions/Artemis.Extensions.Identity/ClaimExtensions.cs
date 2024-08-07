using System.Security.Claims;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;
using Microsoft.AspNetCore.Http;

namespace Artemis.Extensions.Identity;

/// <summary>
/// 凭据扩展
/// </summary>
public static class ClaimExtensions
{
    /// <summary>
    /// 获取凭据列表
    /// </summary>
    /// <param name="record"></param>
    /// <param name="token"></param>
    /// <param name="actionName"></param>
    /// <param name="routePath"></param>
    /// <returns></returns>
    internal static IEnumerable<Claim> GetClaims(this TokenRecord record, string token, string actionName, string routePath)
    {
        var user = new List<Claim>
        {
            new(ArtemisClaimTypes.Authorization, token),
            new(ArtemisClaimTypes.UserId, record.UserId.GuidToString()),
            new(ArtemisClaimTypes.UserName, record.UserName),
            new(ArtemisClaimTypes.EndType, record.EndType)
        };

        var roles = record.Roles
            .Select(item => new Claim(ArtemisClaimTypes.Role, item.Name));
        user.AddRange(roles);

        //claim document
        var userClaims = record.UserClaims;
        var roleClaims = record.RoleClaims;
        var claims = userClaims.Concat(roleClaims)
            .Select(item => new Claim(item.ClaimType, item.ClaimValue));

        user.AddRange(claims);

        //mate data
        user.Add(new Claim(ArtemisClaimTypes.MateActionName, actionName));

        user.Add(new Claim(ArtemisClaimTypes.MateRoutePath, routePath));

        return user;
    }


    /// <summary>
    /// 获取凭据列表
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static IEnumerable<Claim> GetClaims(this HttpContext httpContext)
    {
        return httpContext.User.Claims;
    }

    /// <summary>
    /// 获取令牌值
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static string? GetToken(this HttpContext httpContext)
    {
        return httpContext
            .GetClaims()
            .Where(claim => claim.Type == ArtemisClaimTypes.Authorization)
            .Select(claim => claim.Value)
            .FirstOrDefault();
    }

    /// <summary>
    /// 获取用户标识
    /// </summary>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    public static (bool, Guid) GetUserId(this HttpContext httpContext)
    {
        var userIdString = httpContext
            .GetClaims()
            .Where(claim => claim.Type == ArtemisClaimTypes.UserId)
            .Select(claim => claim.Value)
            .FirstOrDefault();

        var valid = Guid.TryParse(userIdString, out var userId);

        return (valid, userId);
    }
}