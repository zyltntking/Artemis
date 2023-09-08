namespace Artemis.Extensions.Web.Builder;

/// <summary>
/// 路由项目
/// </summary>
internal class RouteItem
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public string Action { get; set; } = null!;

    /// <summary>
    /// 路由方法
    /// </summary>
    public string? Methods { get; set; }

    /// <summary>
    /// 路由路径
    /// </summary>
    public string Path { get; set; } = null!;

    /// <summary>
    /// 域操作名
    /// </summary>
    public string DomainAction { get; set; } = null!;

    /// <summary>
    /// 接口描述
    /// </summary>
    public string? Description { get; set; }
}