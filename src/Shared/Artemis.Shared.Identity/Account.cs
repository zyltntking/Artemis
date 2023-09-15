﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using Artemis.Data.Core;

namespace Artemis.Shared.Identity;

/// <summary>
///     账户服务接口
/// </summary>
[ServiceContract]
public interface IAccount
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
public record SignInRequest
{
    /// <summary>
    ///     用户名
    /// </summary>
    [DataMember(Order = 1)]
    public string UserName { get; set; } = null!;

    /// <summary>
    ///     密码
    /// </summary>
    [DataMember(Order = 2)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}

/// <summary>
///     登录响应
/// </summary>
[DataContract]
public record SignInReply
{
    /// <summary>
    ///     登录Token
    /// </summary>
    [DataMember(Order = 1)]
    public string Token { get; set; } = null!;

    /// <summary>
    ///     Token过期时间
    /// </summary>
    [DataMember(Order = 2)]
    public DateTime Expire { get; set; }
}