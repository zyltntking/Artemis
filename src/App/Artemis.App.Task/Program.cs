using Artemis.Extensions.Identity;
using Artemis.Extensions.ServiceConnect;
using Artemis.Service.Shared;
using Artemis.Service.Task;
using Artemis.Service.Task.Context;
using Artemis.Service.Task.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Artemis.App.Task;

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

            builder.AddServiceCommons();

            // Add services to the container.
            builder.AddRedisComponent("RedisInstance");
            //builder.AddMongoDbComponent("MongoInstance");
            //builder.AddRabbitMqComponent("RabbitMqInstance");

            builder.AddPostgreSqlComponent<TaskContext>("ArtemisDb", optionsBuilder =>
                {
                    optionsBuilder.MigrationsHistoryTable("TaskDbHistory", Project.Schemas.Task);
                    optionsBuilder.MigrationsAssembly("Artemis.App.Task");
                }, Log.Debug)
                .AddTaskServices();

            //配置认证
            builder.Services.AddAuthentication()
                .AddScheme<ArtemisAuthenticationOptions, ArtemisAuthenticationHandler>("Artemis", _ => { });

            //配置授权
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

            // 配置 Grpc 服务， 包括swagger文档配置和验证器配置
            builder.ConfigureGrpc(!isMigration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.ConfigureAppCommons();

            // Use Grpc Swagger Document
            app.UseGrpcSwagger();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<TaskServiceImplement>();

            // map common endpoints
            app.MapCommonEndpoints<TaskContext>();

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