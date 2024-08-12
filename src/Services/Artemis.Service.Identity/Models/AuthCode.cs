using System.ComponentModel.DataAnnotations;
using Artemis.Data.Core;
using Artemis.Service.Shared.Identity;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Identity.Models;

/// <summary>
/// 验证码模型
/// </summary>
public class AuthCode : ModelBase, IAuthCode
{
    #region Implementation of IAuthCodePackage

    /// <summary>
    /// 签名
    /// </summary>
    [Required]
    [Comment("签名")]
    [MaxLength(128)]
    public required string Sign { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [Required]
    [Comment("验证码")]
    [StringLength(128)]
    public required string Code { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    [Comment("发送时间")]
    public required DateTime SendTime { get; set; } = DateTime.Now;

    /// <summary>
    /// 过期时间
    /// </summary>
    [Comment("过期时间")]
    public required DateTime ExpireTime { get; set; }


    /// <summary>
    /// 是否使用
    /// </summary>
    [Comment("是否使用")]
    public bool Used { get; set; } = false;

    #endregion
}