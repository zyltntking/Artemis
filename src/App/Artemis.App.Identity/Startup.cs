using Artemis.App.Identity.Services;
using Artemis.Extensions.Web.Identity;
using Artemis.Extensions.Web.Serilog;
using Artemis.Services.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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

        builder.Services.AddGrpc().AddJsonTranscoding();
        builder.Services.AddGrpcReflection();
        builder.Services.AddGrpcSwagger();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
        });

        builder.Services.AddArtemisAuthorization(new ArtemisAuthorizationOptions
        {
            EnableAdvancedPolicy = false,
            HeaderTokenKey = Constants.HeaderTokenKey,
            CacheTokenPrefix = Constants.CacheTokenPrefix,
            Expire = 604800
        });

        builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });


        //builder.Services.AddControllers();
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen(options =>
        //{
        //    options.SwaggerDoc("v1",
        //        new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
        //});

        //builder.Services.AddCodeFirstGrpc(options =>
        //{
        //    options.EnableDetailedErrors = true;
        //    options.Interceptors.Add<AddInLogInterceptor>();
        //});

        //if (builder.Environment.IsDevelopment())
        //    builder.Services.AddCodeFirstGrpcReflection();
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

        //app.UseSwagger(options => { });
        //app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseResponseCompression();

        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
        app.MapGrpcService<GreeterService>();

        //app.MapDefaultArtemisIdentityGrpcServices();

        if (app.Environment.IsDevelopment())
        {
            app.MapGet("api-route-table", (IEnumerable<EndpointDataSource> endpointSources, HttpContext context) =>
            {
                var end = endpointSources.SelectMany(es => es.Endpoints);
            });

            app.MapGrpcReflectionService();
        }
        //app.MapApiRouteTable();
        //app.MapCodeFirstGrpcReflectionService();
    }

    #endregion
}