using Artemis.App.Identity.Services;
using Artemis.Extensions.ServiceConnect;
using Artemis.Extensions.ServiceConnect.MapEndPoints;
using Artemis.Extensions.ServiceConnect.SwaggerFilters;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Managers;
using Artemis.Service.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Serilog;

namespace Artemis.App.Identity;

/// <summary>
///     应用程序入口
/// </summary>
public class Program
{
    /// <summary>
    ///     主函数
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting web application");

            var builder = WebApplication.CreateBuilder(args);
            builder.AddAspireConfiguration();

            builder.ConfigureSerilog();

            builder.AddServiceDefaults();
            builder.AddServiceCommons();

            // Add services to the container.
            builder.AddRedisComponent("RedisInstance");
            //builder.AddMongoDbComponent("MongoInstance");
            //builder.AddRabbitMqComponent("RabbitMqInstance");

            builder.AddPostgreSqlComponent<IdentityContext>("ArtemisDb");

            builder.Services.AddScoped<IIdentityClaimStore, IdentityClaimStore>();
            builder.Services.AddScoped<IIdentityUserStore, IdentityUserStore>();
            builder.Services.AddScoped<IIdentityRoleStore, IdentityRoleStore>();
            builder.Services.AddScoped<IIdentityRoleClaimStore, IdentityRoleClaimStore>();
            builder.Services.AddScoped<IIdentityUserRoleStore, IdentityUserRoleStore>();
            builder.Services.AddScoped<IIdentityUserClaimStore, IdentityUserClaimStore>();
            builder.Services.AddScoped<IIdentityUserLoginStore, IdentityUserLoginStore>();
            builder.Services.AddScoped<IIdentityUserTokenStore, IdentityUserTokenStore>();

            builder.Services.AddScoped<IIdentityUserManager, IdentityUserManager>();
            builder.Services.AddScoped<IIdentityRoleManager, IdentityRoleManager>();
            builder.Services.AddScoped<IIdentityAccountManager, IdentityAccountManager>();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequiredLength = 6,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true,
                    RequiredUniqueChars = 1
                };
            });

            var swaggerConfig = new SwaggerConfig
            {
                AppName = "Identity Service",
                XmlDocs =
                [
                    Path.Combine(AppContext.BaseDirectory, "Artemis.Protos.Identity.xml"),
                    Path.Combine(AppContext.BaseDirectory, "Artemis.Service.Identity.xml")
                ]
            };

            // 配置 Grpc 服务， 包括swagger文档配置和验证器配置
            builder.ConfigureGrpc(swaggerConfig);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.ConfigureAppCommon();
            // Use Grpc Swagger Document
            app.UseGrpcModify(swaggerConfig);

            // Configure the HTTP request pipeline.
            app.MapGrpcService<AccountService>();
            // app.MapGrpcService<UserService>();

            // map default endpoints for health check
            app.MapDefaultEndpoints();

            // map migration endpoint through "/migrate"
            app.MapMigrationEndpoint<IdentityContext>();

            app.Run();
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "An unhandled exception occurred during bootstrapping");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}