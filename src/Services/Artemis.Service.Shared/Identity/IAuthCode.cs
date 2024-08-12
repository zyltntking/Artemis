using Artemis.Data.Core;

namespace Artemis.Service.Shared.Identity;

/// <summary>
/// 验证码接口
/// </summary>
public interface IAuthCode : IAuthCodeInfo
{
}

/// <summary>
/// 验证码信息接口
/// </summary>
public interface IAuthCodeInfo : IAuthCodePackage, IKeySlot
{
}


/// <summary>
/// 验证码数据包接口
/// </summary>
public interface IAuthCodePackage
{
    /// <summary>
    /// 签名
    /// </summary>
    string Sign { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    string Code { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    DateTime SendTime { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    DateTime ExpireTime { get; set; }

    /// <summary>
    /// 是否使用
    /// </summary>
    bool Used { get; set; }
}