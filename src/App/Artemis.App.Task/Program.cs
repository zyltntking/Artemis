
using Artemis.Extensions.ServiceConnect;
using Artemis.Extensions.ServiceConnect.Authorization;
using Artemis.Service.Shared;
using Artemis.Service.Task;
using Artemis.Service.Task.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Artemis.App.Task;

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
            builder.AddAspireConfiguration();

            builder.ConfigureSerilog();

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
                .AddTaskServices<ArtemisHandlerProxy>();

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