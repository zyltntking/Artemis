using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Artemis.Extensions.Rpc;

/// <summary>
///     Web扩展
/// </summary>
public static class WebExtensions
{
    /// <summary>
    ///     添加验证器
    /// </summary>
    /// <typeparam name="TValidator"></typeparam>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    /// <exception cref="AggregateException"></exception>
    public static IServiceCollection AddValidator<TValidator>(this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton) where TValidator : class
    {
        var implementationType = typeof(TValidator);
        var validatorType = implementationType.GetInterfaces()
            .FirstOrDefault(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>));

        if (validatorType == null)
            throw new AggregateException(implementationType.Name + "没有继承自 IValidator<>.");

        var messageType = validatorType.GetGenericArguments().First();
        var serviceType = typeof(IValidator<>).MakeGenericType(messageType);

        services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
        return services;
    }
}