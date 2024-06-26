using Artemis.Extensions.ServiceConnect;
using Artemis.Extensions.ServiceConnect.Authorization;
using Artemis.Service.School;
using Artemis.Service.School.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Artemis.App.School;

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

            builder.AddServiceCommons();

            // Add services to the container.
            builder.AddRedisComponent("RedisInstance");
            //builder.AddMongoDbComponent("MongoInstance");
            //builder.AddRabbitMqComponent("RabbitMqInstance");

            builder.AddPostgreSqlComponent<SchoolContext>("ArtemisDb", optionsBuilder =>
                {
                    optionsBuilder.MigrationsHistoryTable("SchoolDbHistory", "school");
                    optionsBuilder.MigrationsAssembly("Artemis.App.School");
                }, Log.Debug)
                .AddSchoolServices<ArtemisHandlerProxy>();

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
            //app.MapGrpcService<SampleService>();
            //app.MapGrpcService<AccountService>();
            //app.MapGrpcService<UserService>();
            //app.MapGrpcService<RoleService>();

            // map common endpoints
            app.MapCommonEndpoints<SchoolContext>();

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