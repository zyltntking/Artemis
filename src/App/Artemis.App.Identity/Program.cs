using Artemis.Extensions.ServiceConnect;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Managers;
using Artemis.Service.Identity.Services;
using Artemis.Service.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();
        builder.AddServiceCommons();

        // Add services to the container.
        builder.AddRedisComponent("RedisInstance");
        builder.AddMongoDbComponent("MongoInstance");
        builder.AddRabbitMqComponent("RabbitMqInstance");

        builder.AddNpgsqlDbContext<IdentityContext>("ArtemisDb", configureDbContextOptions: config =>
        {
            config.UseNpgsql(options =>
                {
                    options.MigrationsHistoryTable("IdentityDbHistory", "identity");
                    options.MigrationsAssembly("Artemis.App.Identity");
                })
                .EnableServiceProviderCaching()
                .EnableDetailedErrors(builder.Environment.IsDevelopment())
                .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
                .LogTo(Console.WriteLine, LogLevel.Information);
        });

        if (builder.Environment.IsDevelopment()) builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.TryAddScoped<IIdentityClaimStore, IdentityClaimStore>();
        builder.Services.TryAddScoped<IIdentityUserStore, IdentityUserStore>();
        builder.Services.TryAddScoped<IIdentityRoleStore, IdentityRoleStore>();
        builder.Services.TryAddScoped<IIdentityRoleClaimStore, IdentityRoleClaimStore>();
        builder.Services.TryAddScoped<IIdentityUserRoleStore, IdentityUserRoleStore>();
        builder.Services.TryAddScoped<IIdentityUserClaimStore, IdentityUserClaimStore>();
        builder.Services.TryAddScoped<IIdentityUserLoginStore, IdentityUserLoginStore>();
        builder.Services.TryAddScoped<IIdentityUserTokenStore, IdentityUserTokenStore>();

        builder.Services.TryAddScoped<IIdentityUserManager, IdentityUserManager>();

        builder.ConfigureGrpc(new GrpcSwaggerConfig
        {
            AppName = "认证服务",
            XmlDocs =
            [
                Path.Combine(AppContext.BaseDirectory, "Artemis.Protos.Identity.xml"),
                Path.Combine(AppContext.BaseDirectory, "Artemis.Service.Identity.xml")
            ]
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.ConfigureAppCommon();
        // Use Grpc Swagger Document
        app.UseGrpcModify();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<AccountService>();
        app.MapGrpcService<UserService>();

        // map default endpoints for health check
        app.MapDefaultEndpoints();

        // map migration endpoint through "/migrate"
        app.MapMigrationEndpoint<IdentityContext>();

        app.Run();
    }
}