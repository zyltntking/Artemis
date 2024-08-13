namespace Artemis.Data.Core.Fundamental;

/// <summary>
/// 文章状态
/// </summary>
public sealed class PostStatus : Enumeration
{


    /// <summary>
    ///     构造函数
    /// </summary>
    /// <param name="id">枚举Id</param>
    /// <param name="name">枚举名称</param>
    private PostStatus(int id, string name) : base(id, name)
    {
    }
}