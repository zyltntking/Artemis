namespace Artemis.App.Swashbuckle.Attributes;

/// <summary>
/// enable hiding parameters from swagger docs
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class SwaggerHideParameterAttribute : Attribute
{
}