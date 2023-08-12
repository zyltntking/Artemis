using Microsoft.AspNetCore.Mvc;

namespace Artemis.Extensions.Web.OpenApi.Attributes;

/// <summary>
///     Api版本范围属性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class ApiVersionRangeAttribute : Attribute
{
    /// <summary>
    ///     Api版本范围
    /// </summary>
    /// <param name="fromVersion">起始支持版本</param>
    /// <param name="toVersion">终止支持版本</param>
    /// <exception cref="ArgumentNullException"></exception>
    public ApiVersionRangeAttribute(string fromVersion, string? toVersion = null)
    {
        if (fromVersion is null) throw new ArgumentNullException(nameof(fromVersion));

        FromVersion = ApiVersion.Parse(fromVersion);

        if (toVersion != null) ToVersion = ApiVersion.Parse(toVersion);
    }

    /// <summary>
    ///     起始支持版本
    /// </summary>
    public ApiVersion FromVersion { get; }

    /// <summary>
    ///     终止支持版本
    /// </summary>
    public ApiVersion? ToVersion { get; }
}