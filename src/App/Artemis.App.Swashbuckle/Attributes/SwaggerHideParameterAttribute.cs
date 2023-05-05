namespace Artemis.App.Swashbuckle.Attributes;

/// <summary>
///     enable hiding parameters from swagger docs
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class SwaggerHideParameterAttribute : Attribute
{
}