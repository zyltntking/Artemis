using Artemis.App.Logic.IdentityLogic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artemis.App.Logic.IdentityLogic;

/// <summary>
/// 认证扩展
/// </summary>
public static class IdentityExtensions
{
    /// <summary>
    /// 添加认证服务
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="logicOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddIdentityService(this IServiceCollection serviceCollection, IdentityLogicOptions logicOptions)
    {
        serviceCollection.AddDbContext<ArtemisIdentityDbContext>(options =>
            {
                options.UseNpgsql(logicOptions.Connection, npgsqlOption =>
                {
                    npgsqlOption.MigrationsHistoryTable("ArtemisIdentityHistory", "identity");

                    npgsqlOption.MigrationsAssembly(logicOptions.AssemblyName);
                }).UseLazyLoadingProxies();
            }).AddDefaultIdentity<ArtemisIdentityUser>()
            .AddEntityFrameworkStores<ArtemisIdentityDbContext>();

        serviceCollection.AddScoped<IIdentityService, IdentityService>();

        serviceCollection.AddDatabaseDeveloperPageExceptionFilter();

        serviceCollection.Configure<HostOptions>(hostOptions =>
        {
            hostOptions.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
        });

        return serviceCollection;
    }
}