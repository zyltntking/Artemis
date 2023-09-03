namespace Artemis.Extensions.Web.Exceptions;

/// <summary>
/// 空凭据异常
/// </summary>
public class ClaimNullException : Exception
{
    /// <summary>
    /// 构造
    /// </summary>
    public ClaimNullException() : base("未能格式化出有效凭据")
    {
    }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="actionName">操作名</param>
    public ClaimNullException(string actionName):base($"操作{actionName}位能格式化出有效凭据")
    {

    }
}