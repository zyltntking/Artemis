using Artemis.App.Identity.Services;
using Artemis.Data.Core;
using Artemis.Extensions.ServiceConnect;
using Artemis.Extensions.ServiceConnect.Authorization;
using Artemis.Extensions.ServiceConnect.MapEndPoints;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Managers;
using Artemis.Service.Identity.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

            builder.AddPostgreSqlComponent<IdentityContext>("ArtemisDb", Log.Debug);

            builder.Services.AddScoped<IHandlerProxy, ArtemisHandlerProxy>();

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
            builder.Services.AddScoped<IIdentityResourceManager, IdentityResourceManager>();

            builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection("IdentityOption"));

            var isMigration = false;

            try
            {
                if (EF.IsDesignTime) isMigration = true;
            }
            catch
            {
                isMigration = true;
            }

            // 配置 Grpc 服务， 包括swagger文档配置和验证器配置
            builder.ConfigureGrpc(!isMigration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.ConfigureAppCommon();
            // Use Grpc Swagger Document
            app.UseGrpcModify();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<SampleService>();
            app.MapGrpcService<AccountService>();
            //app.MapGrpcService<UserService>();
            //app.MapGrpcService<RoleService>();

            // map default endpoints for health check
            app.MapDefaultEndpoints();

            // map migration endpoint through "/migrate"
            app.MapMigrationEndpoint<IdentityContext>();

            // map route table endpoint through "/route-table"
            app.MapRouteTable();

            app.Run();
        }
        catch (Exception exception) when (exception is not HostAbortedException &&
                                          exception.Source != "Microsoft.EntityFrameworkCore.Design")
        {
            Log.Fatal(exception, "An unhandled exception occurred during bootstrapping");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}