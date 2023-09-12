using System.ComponentModel.DataAnnotations;
using ProtoBuf;

namespace Artemis.App.IdentityApplication.Protocols;

/// <summary>
/// 注册请求
/// </summary>
[ProtoContract]
public class SignUpRequest
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required]
    public string Username { get; set; } = null!;

    /// <summary>
    /// 密码
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}

/// <summary>
/// 登录响应
/// </summary>
public class SignInResponse
{
    /// <summary>
    /// 令牌
    /// </summary>
    public string Token { get; set; } = null!;

    /// <summary>
    /// 释放时间
    /// </summary>
    public DateTime Expire { get; set; } = DateTime.Now;
}