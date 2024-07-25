using Artemis.Extensions.Identity;
using Artemis.Extensions.ServiceConnect;
using Artemis.Service.RawData.Context;
using Artemis.Service.School;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Artemis.App.RawData;

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
            //builder.AddMongoDbComponent("MongoInstance");
            //builder.AddRabbitMqComponent("RabbitMqInstance");

            builder.AddPostgreSqlComponent<RawDataContext>("ArtemisDb", optionsBuilder =>
            {
                optionsBuilder.MigrationsHistoryTable("RawDataDbHistory", Project.Schemas.RawData);
                optionsBuilder.MigrationsAssembly("Artemis.App.RawData");
            }, Log.Debug).AddRawDataServices();

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
            //app.MapGrpcService<SampleService>();
            //app.MapGrpcService<AccountService>();
            //app.MapGrpcService<UserService>();
            //app.MapGrpcService<RoleService>();

            // map common endpoints
            app.MapCommonEndpoints<RawDataContext>();

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