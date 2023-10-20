using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Core;
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
    /// <param name="request">登录请求</param>
    /// <returns>登录响应<see cref="SignInReply" /></returns>
    [OperationContract]
    Task<DataResult<SignInReply>> SignIn(SignInRequest request);
}

/// <summary>
///     登录请求
/// </summary>
[DataContract]
public record SignInRequest : SignInPackage
{
    /// <summary>
    ///     用户名
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
///     登录响应
/// </summary>
[DataContract]
public record SignInReply : TokenPackage
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
    public override required DateTime Expire { get; set; }
}