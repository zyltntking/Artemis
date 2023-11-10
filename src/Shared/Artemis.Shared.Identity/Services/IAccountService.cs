using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Grpc;
using Artemis.Shared.Identity.Transfer;

namespace Artemis.Shared.Identity.Services;

/// <summary>
///     账户服务接口
/// </summary>
[ServiceContract]
public interface IAccountService
{
    /// <summary>
    ///     登录
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("登录")]
    Task<TokenResponse> SignInAsync(SignInRequest request);

    /// <summary>
    ///     注册
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("注册")]
    Task<TokenResponse> SignUpAsync(SignUpRequest request);

    /// <summary>
    ///     修改密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("修改密码")]
    Task<GrpcEmptyResponse> ChangePasswordAsync(ChangePasswordRequest request);

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("重置密码")]
    Task<GrpcEmptyResponse> ResetPasswordAsync(ResetPasswordRequest request);

    /// <summary>
    ///     重置密码
    /// </summary>
    /// <param name="request">请求</param>
    /// <returns></returns>
    [OperationContract]
    [Description("重置密码")]
    Task<GrpcEmptyResponse> ResetPasswordsAsync(ResetPasswordsRequest request);
}

/// <summary>
///     登录请求
/// </summary>
[DataContract]
public sealed record SignInRequest : SignInPackage
{
    /// <summary>
    ///     用户签名
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required string UserSign { get; set; }

    /// <summary>
    ///     密码
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    [DataType(DataType.Password)]
    public override required string Password { get; set; }
}

/// <summary>
///     注册请求
/// </summary>
[DataContract]
public sealed record SignUpRequest : UserPackage
{
    #region Implementation of RolePackage

    /// <summary>
    ///     用户名
    /// </summary>
    [Required]
    [MaxLength(32)]
    [DataMember(Order = 1)]
    public override required string UserName { get; set; }

    /// <summary>
    ///     密码
    /// </summary>
    [Required]
    [StringLength(32, MinimumLength = 6)]
    [DataMember(Order = 2)]
    public required string Password { get; set; }

    /// <summary>
    ///     电子邮件
    /// </summary>
    [EmailAddress]
    [MaxLength(128)]
    [DataMember(Order = 3)]
    public override string? Email { get; set; }

    /// <summary>
    ///     电话号码
    /// </summary>
    [Phone]
    [MaxLength(16)]
    [DataMember(Order = 4)]
    public override string? PhoneNumber { get; set; }

    #endregion
}

/// <summary>
///     Token结果
/// </summary>
[DataContract]
public sealed record TokenResult : TokenPackage
{
    /// <summary>
    ///     登录Token
    /// </summary>
    [DataMember(Order = 1)]
    public override string? Token { get; set; }

    /// <summary>
    ///     Token过期时间
    /// </summary>
    [DataMember(Order = 2)]
    public override required long Expire { get; set; }
}

/// <summary>
///     Token响应
/// </summary>
[DataContract]
public sealed record TokenResponse : GrpcResponse<TokenResult>
{
    #region Overrides of GrpcResponse<TokenResult>

    /// <summary>
    ///     结果信息
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public override required GrpcPageResult Result { get; set; }

    /// <summary>
    ///     结果数据
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public override required TokenResult Data { get; set; }

    #endregion
}

/// <summary>
///     修改密码请求
/// </summary>
[DataContract]
public sealed record ChangePasswordRequest
{
    /// <summary>
    ///     用户签名
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required string UserSign { get; set; }

    /// <summary>
    ///     原始密码
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required string OldPassword { get; set; }

    /// <summary>
    ///     新密码
    /// </summary>
    [Required]
    [DataMember(Order = 3)]
    public required string NewPassword { get; set; }
}

/// <summary>
///     重置密码请求
/// </summary>
[DataContract]
public sealed record ResetPasswordRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required Guid UserId { get; set; }

    /// <summary>
    ///     密码
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required string Password { get; set; }
}

/// <summary>
///     重置密码请求
/// </summary>
[DataContract]
public sealed record ResetPasswordsRequest
{
    /// <summary>
    ///     用户标识
    /// </summary>
    [Required]
    [DataMember(Order = 1)]
    public required List<Guid> UserIds { get; set; }

    /// <summary>
    ///     密码
    /// </summary>
    [Required]
    [DataMember(Order = 2)]
    public required string Password { get; set; }
}