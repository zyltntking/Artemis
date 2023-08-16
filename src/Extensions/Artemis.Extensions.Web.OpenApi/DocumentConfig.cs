namespace Artemis.Extensions.Web.OpenApi;

/// <summary>
///     OpenApi文档配置
/// </summary>
public sealed class DocumentConfig
{
    /// <summary>
    ///     验证配置合法性，若非法则抛出异常
    /// </summary>
    /// <exception cref="ArgumentNullException">参数空异常</exception>
    /// <exception cref="InvalidOperationException">非法操作异常</exception>
    public void EnsureValidity()
    {
    }
}