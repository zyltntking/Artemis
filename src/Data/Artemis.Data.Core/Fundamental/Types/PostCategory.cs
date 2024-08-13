using System.ComponentModel;

namespace Artemis.Data.Core.Fundamental.Types;

/// <summary>
/// 文章分类
/// </summary>
[Description("文章分类")]
public sealed class PostCategory : Enumeration
{
    /// <summary>
    /// 新闻
    /// </summary>
    [Description("新闻")]
    public static PostCategory News = new PostCategory(0, nameof(News));

    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private PostCategory(int id, string name) : base(id, name)
    {
    }
}