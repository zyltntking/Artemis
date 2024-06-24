namespace Artemis.Data.Shared.Transfer;

/// <summary>
///     端类型
/// </summary>
public static class EndType
{
    /// <summary>
    ///     签到初始化
    /// </summary>
    public const string SignInitial = nameof(SignInitial);

    /// <summary>
    ///     Web端
    /// </summary>
    public const string Web = nameof(Web);

    /// <summary>
    ///     App端
    /// </summary>
    public const string App = nameof(App);

    /// <summary>
    ///     微信端
    /// </summary>
    public const string WeChat = nameof(WeChat);

    /// <summary>
    ///     微信小程序端
    /// </summary>
    public const string WxApp = nameof(WxApp);
}