using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     Artemis凭据类型
/// </summary>
[Description("Artemis凭据类型")]
public sealed class ArtemisClaimTypes : Enumeration
{
    /// <summary>
    ///     用户标识凭据
    /// </summary>
    [Description("用户标识凭据")] public static readonly ArtemisClaimTypes UserId = new(1, nameof(UserId));

    /// <summary>
    ///     用户名凭据
    /// </summary>
    [Description("用户名凭据")] public static readonly ArtemisClaimTypes UserName = new(2, nameof(UserName));

    /// <summary>
    ///     端类型凭据
    /// </summary>
    [Description("端类型凭据")] public static readonly ArtemisClaimTypes EndType = new(3, nameof(EndType));

    /// <summary>
    ///     角色标识凭据
    /// </summary>
    [Description("角色标识凭据")] public static readonly ArtemisClaimTypes Role = new(4, nameof(Role));

    /// <summary>
    ///     认证令牌凭据
    /// </summary>
    [Description("认证令牌凭据")] public static readonly ArtemisClaimTypes Authorization = new(5, nameof(Authorization));

    /// <summary>
    ///     路由凭据
    /// </summary>
    [Description("路由凭据")] public static readonly ArtemisClaimTypes RoutePath = new(6, nameof(RoutePath));

    /// <summary>
    ///     元路由凭据
    /// </summary>
    [Description("元路由凭据")] public static readonly ArtemisClaimTypes MateRoutePath = new(7, nameof(MateRoutePath));

    /// <summary>
    ///     操作名凭据
    /// </summary>
    [Description("操作名凭据")] public static readonly ArtemisClaimTypes ActionName = new(8, nameof(ActionName));

    /// <summary>
    ///     元操作名凭据
    /// </summary>
    [Description("元操作名凭据")] public static readonly ArtemisClaimTypes MateActionName = new(9, nameof(MateActionName));

    /// <summary>
    ///     签名凭据
    /// </summary>
    [Description("签名凭据")] public static readonly ArtemisClaimTypes Signature = new(10, nameof(Signature));

    /// <summary>
    ///     Ip地址凭据
    /// </summary>
    [Description("Ip地址凭据")] public static readonly ArtemisClaimTypes IpAddress = new(11, nameof(IpAddress));

    /// <summary>
    ///     Dns凭据
    /// </summary>
    [Description("Dns凭据")] public static readonly ArtemisClaimTypes Dns = new(12, nameof(Dns));

    /// <summary>
    ///     Mac地址凭据
    /// </summary>
    [Description("Mac地址凭据")] public static readonly ArtemisClaimTypes MacAddress = new(13, nameof(MacAddress));

    /// <summary>
    ///     设备标识凭据
    /// </summary>
    [Description("设备标识凭据")] public static readonly ArtemisClaimTypes DeviceId = new(14, nameof(DeviceId));

    /// <summary>
    ///     设备名称凭据
    /// </summary>
    [Description("设备名称凭据")] public static readonly ArtemisClaimTypes DeviceName = new(15, nameof(DeviceName));

    /// <summary>
    ///     设备类型凭据
    /// </summary>
    [Description("设备类型凭据")] public static readonly ArtemisClaimTypes DeviceType = new(16, nameof(DeviceType));

    /// <summary>
    ///     邮箱凭据
    /// </summary>
    [Description("邮箱凭据")] public static readonly ArtemisClaimTypes Email = new(17, nameof(Email));

    /// <summary>
    ///     移动电话凭据
    /// </summary>
    [Description("移动电话凭据")] public static readonly ArtemisClaimTypes Phone = new(18, nameof(Phone));

    /// <summary>
    ///     地址凭据
    /// </summary>
    [Description("地址凭据")] public static readonly ArtemisClaimTypes Address = new(19, nameof(Address));

    /// <summary>
    ///     性别凭据
    /// </summary>
    [Description("性别凭据")] public static readonly ArtemisClaimTypes Gender = new(20, nameof(Gender));

    /// <summary>
    ///     生日凭据
    /// </summary>
    [Description("生日凭据")] public static readonly ArtemisClaimTypes Birthday = new(21, nameof(Birthday));

    /// <summary>
    ///     过期时间凭据
    /// </summary>
    [Description("过期时间凭据")] public static readonly ArtemisClaimTypes Expired = new(22, nameof(Expired));

    /// <summary>
    ///     截止日期凭据
    /// </summary>
    [Description("截止日期凭据")] public static readonly ArtemisClaimTypes Expiration = new(23, nameof(Expiration));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private ArtemisClaimTypes(int id, string name) : base(id, name)
    {
    }
}