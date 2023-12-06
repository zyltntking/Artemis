using Artemis.App.Identity.Interceptors;
using Artemis.Extensions.Web.Builder;
using Artemis.Extensions.Web.Middleware;
using Artemis.Extensions.Web.OpenApi;
using Artemis.Extensions.Web.Serilog;
using Artemis.Services.Identity;
using Artemis.Services.Identity.Logic;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc.Server;

namespace Artemis.App.Identity;

/// <summary>
///     启动器配置
/// </summary>
public class Startup : IWebAppStartup
{
    /// <summary>
    ///     启动器构造
    /// </summary>
    public Startup()
    {
        DocConfig = new DocumentConfig();
    }

    private DocumentConfig DocConfig { get; }

    #region Implementation of IWebAppStartup

    /// <summary>
    ///     配置服务
    /// </summary>
    /// <param name="builder">程序集</param>
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("IdentityContext") ??
                               throw new InvalidOperationException("ContextConnection string 'Identity' not found.");

        builder.Services.AddIdentityService(new IdentityServiceOptions
        {
            EnableCache = true,
            RedisCacheConnection = builder.Configuration.GetConnectionString("RedisCache"),
            RegisterDbAction = dbBuilder =>
            {
                dbBuilder.UseNpgsql(connectionString, npgsqlOption =>
                {
                    npgsqlOption.MigrationsHistoryTable("ArtemisIdentityHistory", "identity");

                    npgsqlOption.MigrationsAssembly("Artemis.App.Identity");

                    npgsqlOption.EnableRetryOnFailure(3);

                    npgsqlOption.CommandTimeout(30);
                });
            }
        }, builder.Environment.IsDevelopment());

        builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });

        builder.Services.AddCodeFirstGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<TokenInterceptor>();
        });

        if (builder.Environment.IsDevelopment()) builder.Services.AddCodeFirstGrpcReflection();

        builder.Services.AddArtemisMiddleWares(options => { options.ServiceDomain.DomainName = "Identity"; });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.AddOpenApiDoc(DocConfig);
    }

    /// <summary>
    ///     应用配置
    /// </summary>
    /// <param name="app"></param>
    public void ConfigureApplication(WebApplication app)
    {
        app.UseOpenApiDoc(DocConfig);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
            app.UseMigrationsEndPoint();
        else
            app.UseExceptionHandler("/Error");

        app.UseRouting();

        app.UseArtemisMiddleWares();
        app.UseResponseCompression();

        app.MapGrpcService<RoleService>();
        app.MapGrpcService<UserService>();
        app.MapGrpcService<AccountService>();

        if (app.Environment.IsDevelopment())
        {
            app.MapApiRouteTable();
            app.MapCodeFirstGrpcReflectionService();
        }
    }

    #endregion
}