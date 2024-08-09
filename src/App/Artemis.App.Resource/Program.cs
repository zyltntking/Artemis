using Artemis.Extensions.Identity;
using Artemis.Extensions.ServiceConnect;
using Artemis.Service.Resource;
using Artemis.Service.Resource.Context;
using Artemis.Service.Resource.Services;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Artemis.App.Resource;

/// <summary>
///     Ӧ�ó������
/// </summary>
public class Program
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

            builder.AddPostgreSqlComponent<ResourceContext>("ArtemisDb", optionsBuilder =>
                {
                    optionsBuilder.MigrationsHistoryTable("ResourceDbHistory", Project.Schemas.Resource);
                    optionsBuilder.MigrationsAssembly("Artemis.App.Resource");
                }, Log.Debug)
                .AddResourceServices();

            builder.ConfigureResourceService();

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
            app.MapGrpcService<DictionaryServiceImplement>();
            app.MapGrpcService<OrganizationServiceImplement>();
            app.MapGrpcService<DivisionServiceImplement>();
            app.MapGrpcService<StandardServiceImplement>();
            app.MapGrpcService<SystemModuleServiceImplement>();

            // map common endpoints
            app.MapCommonEndpoints<ResourceContext>();

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