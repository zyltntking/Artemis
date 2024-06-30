using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Artemis.Extensions.ServiceConnect.Validators;

/// <summary>
///     Web扩展
/// </summary>
internal static class ValidatorExtensions
{
    /// <summary>
    ///     添加验证器
    /// </summary>
    /// <param name="services"></param>
    /// <param name="implementationType"></param>
    /// <param name="lifetime"></param>
    /// <exception cref="AggregateException"></exception>
    private static void AddValidator(
        IServiceCollection services,
        Type implementationType,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        var validatorType = implementationType.GetInterfaces()
                                .FirstOrDefault(type =>
                                    type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IValidator<>))
                            ?? throw new AggregateException(implementationType.Name + "没有继承自 IValidator<>.");

        var messageType = validatorType.GetGenericArguments().First();
        var serviceType = typeof(IValidator<>).MakeGenericType(messageType);

        services.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));
    }

    /// <summary>
    ///     添加验证器
    /// </summary>
    /// <typeparam name="TValidator"></typeparam>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    /// <exception cref="AggregateException"></exception>
    internal static IServiceCollection AddValidator<TValidator>(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton) where TValidator : class
    {
        var implementationType = typeof(TValidator);

        AddValidator(services, implementationType, lifetime);

        return services;
    }

    /// <summary>
    ///     添加当前域下所有验证器
    /// </summary>
    /// <param name="services"></param>
    /// <param name="lifetime"></param>
    /// <returns></returns>
    /// <exception cref="AggregateException"></exception>
    internal static IServiceCollection AddValidators(
        this IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Singleton)
    {
        var implementationTypes = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(t => t.GetInterface(typeof(IValidator<>).FullName!) != null)
            .Where(t => !t.Name.Contains("AbstractValidator"))
            .ToList();

        foreach (var implementationType in implementationTypes)
        {
            AddValidator(services, implementationType, lifetime);
        }

        return services;
    }
}