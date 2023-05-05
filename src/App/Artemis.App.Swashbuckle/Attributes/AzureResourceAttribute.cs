namespace Artemis.App.Swashbuckle.Attributes;

/// <summary>
///     Indicates the object is a ARM Resource.
/// </summary>
/// <see href="https://github.com/Azure/autorest/tree/master/docs/extensions#x-ms-azure-resource">ARM Resource.</see>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class AzureResourceAttribute : Attribute
{
}