namespace Artemis.Extensions.Web.OpenApi.Standards;

/// <summary>
///     自定义模式名称提供程序接口
/// </summary>
public interface ICustomSchemaNameProvider
{
    /// <summary>
    ///     获取名称
    /// </summary>
    /// <param name="type">类型</param>
    /// <returns>模式名称</returns>
    string GetName(Type type);
}