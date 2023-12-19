using Artemis.Extensions.Rpc;
using Artemis.Extensions.Web.Identity;
using Artemis.Extensions.Web.Serilog;
using Artemis.Services.Identity;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc.Server;

namespace Artemis.App.Identity;

/// <summary>
///     启动器配置
/// </summary>
public class Startup : IWebAppStartup
{
    #region Implementation of IWebAppStartup

    /// <summary>
    ///     配置服务
    /// </summary>
    /// <param name="builder">程序集</param>
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Add services to the container.
        var pgsqlConnectionString = builder.Configuration.GetConnectionString("IdentityContext") ??
                                    throw new InvalidOperationException(
                                        "ContextConnection string 'Identity' not found.");

        builder.Services.AddArtemisIdentityService(new IdentityServiceOptions
        {
            EnableCache = true,
            RegisterDbAction = dbBuilder =>
            {
                dbBuilder.UseNpgsql(pgsqlConnectionString, npgsqlOption =>
                {
                    npgsqlOption.MigrationsHistoryTable("ArtemisIdentityHistory", "identity");

                    npgsqlOption.MigrationsAssembly("Artemis.App.Identity");

                    npgsqlOption.EnableRetryOnFailure(3);

                    npgsqlOption.CommandTimeout(30);
                });
            }
        }, builder.Environment.IsDevelopment());

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
        });

        builder.Services.AddArtemisAuthorization(new ArtemisAuthorizationOptions
        {
            EnableAdvancedPolicy = false,
            HeaderTokenKey = Constants.HeaderTokenKey,
            CacheTokenPrefix = Constants.CacheTokenPrefix,
            Expire = 604800
        });

        builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });

        builder.Services.AddCodeFirstGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<AddInLogInterceptor>();
        });

        if (builder.Environment.IsDevelopment())
            builder.Services.AddCodeFirstGrpcReflection();
    }

    /// <summary>
    ///     应用配置
    /// </summary>
    /// <param name="app"></param>
    public void ConfigureApplication(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseResponseCompression();

        app.MapDefaultArtemisIdentityGrpcServices();

        if (app.Environment.IsDevelopment())
            //app.MapApiRouteTable();
            app.MapCodeFirstGrpcReflectionService();
    }

    #endregion
}