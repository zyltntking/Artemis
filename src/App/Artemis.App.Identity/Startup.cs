using Artemis.App.Identity.Interceptors;
using Artemis.Extensions.Web.Identity;
using Artemis.Extensions.Web.Serilog;
using Artemis.Services.Identity;
using Artemis.Services.Identity.Logic;
using Microsoft.AspNetCore.Authorization;
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

        builder.Services.AddIdentityService(new IdentityServiceOptions
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

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Anonymous", policy => { policy.Requirements.Add(new AnonymousRequirement()); });

            options.AddPolicy("Token", policy => { policy.Requirements.Add(new TokenRequirement()); });

            options.AddPolicy("Admin", policy =>
            {
                policy.Requirements.Add(new RolesRequirement(new[]
                {
                    "Admin"
                }));
            });

            options.AddPolicy("LeftIsRight", policy =>
            {
                policy.Requirements.Add(new ClaimRequirement(new[]
                {
                    new KeyValuePair<string, string>("Left", "Right")
                }));
            });

            options.AddPolicy("ActionName", policy => { policy.Requirements.Add(new ActionNameClaimRequirement()); });

            options.AddPolicy("RoutePath", policy => { policy.Requirements.Add(new RoutePathClaimRequirement()); });
        });
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddSingleton<IAuthorizationHandler, ArtemisIdentityHandler>();


        builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });

        builder.Services.AddCodeFirstGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<TokenInterceptor>();
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

        app.MapGrpcService<RoleService>();
        app.MapGrpcService<UserService>();
        app.MapGrpcService<AccountService>();

        if (app.Environment.IsDevelopment())
            //app.MapApiRouteTable();
            app.MapCodeFirstGrpcReflectionService();
    }

    #endregion
}