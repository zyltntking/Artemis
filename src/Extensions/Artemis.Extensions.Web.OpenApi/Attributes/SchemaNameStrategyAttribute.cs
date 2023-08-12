using Artemis.Extensions.Web.OpenApi.Standards;

namespace Artemis.Extensions.Web.OpenApi.Attributes;

/// <summary>
///     模式名称策略属性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum, AllowMultiple = true)]
public sealed class SchemaNameStrategyAttribute : Attribute
{
    /// <summary>
    ///     模式名称策略属性构造函数
    /// </summary>
    /// <param name="namingStrategy">命名规则</param>
    /// <param name="customNameProvider">自定义名称提供程序</param>
    /// <exception cref="ArgumentNullException">空参数异常</exception>
    public SchemaNameStrategyAttribute(NamingStrategy namingStrategy, Type? customNameProvider = null)
    {
        if (namingStrategy == NamingStrategy.Custom && (customNameProvider is null ||
                                                        !typeof(ICustomSchemaNameProvider).IsAssignableFrom(
                                                            customNameProvider)))
            throw new ArgumentNullException(nameof(customNameProvider));

        NamingStrategy = namingStrategy;
        CustomNameProvider = (ICustomSchemaNameProvider)Activator.CreateInstance(customNameProvider!)!;
    }

    /// <summary>
    ///     模式名称策略属性构造函数
    /// </summary>
    /// <param name="name">名称</param>
    public SchemaNameStrategyAttribute(string name)
    {
        NamingStrategy = NamingStrategy.Custom;
        CustomNameProvider = new ConstNameProvider(name);
    }

    /// <summary>
    ///     模式名称策略属性构造函数
    /// </summary>
    /// <param name="namingStrategy">命名策略</param>
    /// <param name="name">名称</param>
    public SchemaNameStrategyAttribute(NamingStrategy namingStrategy, string name)
    {
        NamingStrategy = namingStrategy;
        CustomNameProvider = new ConstNameProvider(name);
    }

    /// <summary>
    ///     命名策略
    /// </summary>
    public NamingStrategy NamingStrategy { get; }

    /// <summary>
    ///     自定义名称提供程序
    /// </summary>
    public ICustomSchemaNameProvider CustomNameProvider { get; private set; }
}

/// <summary>
///     命名策略
/// </summary>
public enum NamingStrategy
{
    /// <summary>
    ///     具名串联
    /// </summary>
    ConcreteNamingConcatanation,

    /// <summary>
    ///     自定义
    /// </summary>
    Custom,

    /// <summary>
    ///     应用到父包装器
    /// </summary>
    ApplyToParentWrapper
}

/// <summary>
///     常量名称提供程序
/// </summary>
public class ConstNameProvider : ICustomSchemaNameProvider
{
    /// <summary>
    ///     常量名称提供程序构造函数
    /// </summary>
    /// <param name="name">名称</param>
    public ConstNameProvider(string name)
    {
        Name = name;
    }

    /// <summary>
    ///     名称
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     获取名称
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public string GetName(Type type)
    {
        return Name;
    }
}