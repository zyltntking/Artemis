namespace Artemis.Extensions.Web.OpenApi;

/// <summary>
///     OpenApi文档信息配置段
/// </summary>
internal sealed class OpenApiDocumentInfo
{
    /// <summary>
    ///     初始化一个<see cref="OpenApiDocumentInfo" />类型的实例
    /// </summary>
    /// <param name="title">OpenApi文档标题</param>
    /// <param name="description">OpenApi文档描述</param>
    /// <param name="version">OpenApi文档版本</param>
    /// <param name="clientName">OpenApi客户端名称</param>
    /// <exception cref="ArgumentNullException"></exception>
    public OpenApiDocumentInfo(string title, string description, string version, string clientName)
    {
        if (string.IsNullOrEmpty(title)) throw new ArgumentNullException(nameof(title));

        if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));

        if (string.IsNullOrEmpty(version)) throw new ArgumentNullException(nameof(version));

        if (string.IsNullOrEmpty(clientName)) throw new ArgumentNullException(nameof(clientName));

        Title = title;
        Description = description;
        Version = version;
        ClientName = clientName;
    }

    /// <summary>
    ///     OpenApi文档信息标题段
    /// </summary>
    public string Title { get; }

    /// <summary>
    ///     OpenApi文档信息描述段
    /// </summary>
    public string Description { get; }

    /// <summary>
    ///     OpenApi文档信息版本段
    /// </summary>
    public string Version { get; }

    /// <summary>
    ///     OpenApi文档信息客户端名称段
    /// </summary>
    public string ClientName { get; }
}