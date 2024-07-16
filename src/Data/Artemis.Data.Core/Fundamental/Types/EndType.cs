using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
///     端类型
/// </summary>
[Description("端类型")]
public sealed class EndType : Enumeration
{
    /// <summary>
    ///     签名初始化
    /// </summary>
    [Description("签名初始化")] public static readonly EndType SignInitial = new(1, nameof(SignInitial));

    /// <summary>
    ///     签入端(临时注册使用)
    /// </summary>
    [Description("签入端")] public static readonly EndType SignUpEnd = new(2, nameof(SignUpEnd));

    /// <summary>
    ///     Web端
    /// </summary>
    [Description("Web端")] public static readonly EndType Web = new(3, nameof(Web));

    /// <summary>
    ///     IOS端
    /// </summary>
    [Description("IOS端")] public static readonly EndType IOS = new(4, nameof(IOS));

    /// <summary>
    ///     Android端
    /// </summary>
    [Description("Android端")] public static readonly EndType Android = new(5, nameof(Android));

    /// <summary>
    ///     微信端
    /// </summary>
    [Description("微信端")] public static readonly EndType WeChat = new(6, nameof(WeChat));

    /// <summary>
    ///     微信小程序端
    /// </summary>
    [Description("微信小程序端")] public static readonly EndType WxApp = new(7, nameof(WxApp));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private EndType(int id, string name) : base(id, name)
    {
    }
}