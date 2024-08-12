using Artemis.Service.Identity.Context.Configuration;
using Artemis.Service.Identity.Models;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Context;

/// <summary>
/// 认证验证码实体
/// </summary>
[EntityTypeConfiguration(typeof(IdentityAuthCodeConfiguration))]
public sealed class IdentityAuthCode : AuthCode
{
    
}