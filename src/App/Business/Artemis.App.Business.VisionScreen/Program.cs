using Artemis.App.Business.VisionScreen.Services;
using Artemis.Extensions.Identity;
using Artemis.Extensions.ServiceConnect;
using Artemis.Service.Business.VisionScreen;
using Artemis.Service.Business.VisionScreen.Context;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Artemis.App.Business.VisionScreen;

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

            builder.AddPostgreSqlComponent<BusinessContext>("ArtemisDb", optionsBuilder =>
                {
                    optionsBuilder.MigrationsHistoryTable("BusinessDbHistory", Project.Schemas.Business);
                    optionsBuilder.MigrationsAssembly("Artemis.App.Business.VisionScreen");
                }, Log.Debug)
                .AddBusinessServices();

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
            app.MapGrpcService<WxParentTerminalServiceImplement>();
            app.MapGrpcService<WxTeacherTerminalServiceImplement>();

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