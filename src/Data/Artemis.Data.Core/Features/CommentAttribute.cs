namespace Artemis.Data.Core.Features;

/// <summary>
///     注释属性
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class CommentAttribute : Attribute
{
    /// <summary>
    ///     注释属性
    /// </summary>
    /// <param name="comment">注释</param>
    public CommentAttribute(string comment)
    {
        Comment = comment;
    }

    /// <summary>
    ///     注释
    /// </summary>
    public string Comment { get; set; }
}