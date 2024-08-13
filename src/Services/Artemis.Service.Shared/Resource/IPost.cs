namespace Artemis.Service.Shared.Resource;

/// <summary>
/// 文章数据包接口
/// </summary>
public interface IPostPackage
{
    /// <summary>
    /// 标题
    /// </summary>
    string Title { get; set; }

    /// <summary>
    /// 分类
    /// </summary>
    string? Category { get; set; }

    /// <summary>
    /// 显示的端类型
    /// </summary>
    string? EndType { get; set; }

    /// <summary>
    /// 文章摘要
    /// </summary>
    string? Summary { get; set; }
}