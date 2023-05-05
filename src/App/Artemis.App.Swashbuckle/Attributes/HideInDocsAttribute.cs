namespace Artemis.App.Swashbuckle.Attributes;

/// <summary>
///     A marker attribute which can be applied to an API or controller to hide from docs.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class HideInDocsAttribute : Attribute
{
}