using Artemis.Extensions.Identity;
using Artemis.Extensions.ServiceConnect;
using Artemis.Service.Identity;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Services;
using Artemis.Service.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Artemis.App.Identity;

/// <summary>
///     Ӧ�ó������
/// </summary>
public abstract class Program
{
    /// <summary>
    ///     ������
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

            builder.AddServiceCommons();

            // Add services to the container.
            builder.AddRedisComponent("RedisInstance");
            builder.AddMongoDbComponent("MongoInstance");
            //builder.AddRabbitMqComponent("RabbitMqInstance");

            builder.AddPostgreSqlComponent<IdentityContext>("ArtemisDb", optionsBuilder =>
                {
                    optionsBuilder.MigrationsHistoryTable("IdentityDbHistory", Project.Schemas.Identity);
                    optionsBuilder.MigrationsAssembly("Artemis.App.Identity");
                }, Log.Debug)
                .AddIdentityServices()
                .Configure<IdentityOptions>(builder.Configuration.GetSection("IdentityOption"));

            //������֤
            builder.Services.AddAuthentication()
                .AddScheme<ArtemisAuthenticationOptions, ArtemisAuthenticationHandler>("Artemis", _ => { });

            //������Ȩ
            builder.ConfigureAuthorization();

            var isMigration = false;

            try
            {
                if (EF.IsDesignTime) isMigration = true;
            }
            catch
            {
                isMigration = true;
            }

            // ���� Grpc ���� ����swagger�ĵ����ú���֤������
            builder.ConfigureGrpc(!isMigration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.ConfigureAppCommons();

            // Use Grpc Swagger Document
            app.UseGrpcSwagger();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<ResourceServiceImplement>();
            app.MapGrpcService<AccountServiceImplement>();
            app.MapGrpcService<UserServiceImplement>();
            app.MapGrpcService<RoleServiceImplement>();

            // map common endpoints
            app.MapCommonEndpoints<IdentityContext>();

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