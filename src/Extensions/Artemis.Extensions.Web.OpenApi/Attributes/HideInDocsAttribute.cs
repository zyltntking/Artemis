namespace Artemis.Extensions.Web.OpenApi.Attributes;

/// <summary>
///     标识需要在文档中隐藏的方法或类
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class HideInDocsAttribute : Attribute
{
}