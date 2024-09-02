using Artemis.Extensions.Identity;
using Artemis.Extensions.ServiceConnect;
using Artemis.Service.Business.VisionScreen;
using Artemis.Service.Business.VisionScreen.Context;
using Artemis.Service.Business.VisionScreen.Services;
using Artemis.Service.Identity;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Services;
using Artemis.Service.Resource;
using Artemis.Service.Resource.Context;
using Artemis.Service.Resource.Services;
using Artemis.Service.School;
using Artemis.Service.School.Context;
using Artemis.Service.School.Services;
using Artemis.Service.Shared;
using Artemis.Service.Task;
using Artemis.Service.Task.Context;
using Artemis.Service.Task.Services;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Artemis.App.Business.VisionScreen;

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

            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 268435456;
            });

            builder.AddServiceCommons();

            // Add services to the container.
            builder.AddRedisComponent("RedisInstance");
            builder.AddMongoDbComponent("MongoInstance");
            //builder.AddRabbitMqComponent("RabbitMqInstance");

            builder.AddPostgreSqlComponent<BusinessContext>("ArtemisDb", optionsBuilder =>
                {
                    optionsBuilder.MigrationsHistoryTable("BusinessDbHistory", Project.Schemas.Business);
                    optionsBuilder.MigrationsAssembly("Artemis.App.Business.VisionScreen");
                }, Log.Debug)
                .AddBusinessServices();

            //builder.AddPostgreSqlComponent<TaskContext>("ArtemisDb")
            //    .AddTaskServices();

            //builder.AddPostgreSqlComponent<SchoolContext>("ArtemisDb")
            //    .AddSchoolServices();

            //builder.AddPostgreSqlComponent<ResourceContext>("ArtemisDb")
            //    .AddResourceServices();
            //builder.ConfigureResourceService();

            //builder.AddPostgreSqlComponent<IdentityContext>("ArtemisDb")
            //    .AddIdentityServices()
            //    .Configure<IdentityOptions>(builder.Configuration.GetSection("IdentityOption"));

            //配置认证
            builder.Services.AddAuthentication()
                .AddScheme<ArtemisAuthenticationOptions, ArtemisAuthenticationHandler>("Artemis", _ => { });

            builder.Services.Configure<FormOptions>(options =>
            {
                // Set the limit to 256 MB
                options.MultipartBodyLengthLimit = 268435456;
            });

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

            //// Identity
            //app.MapGrpcService<ResourceServiceImplement>();
            //app.MapGrpcService<AccountServiceImplement>();
            //app.MapGrpcService<UserServiceImplement>();
            //app.MapGrpcService<RoleServiceImplement>();

            //// Resource
            //app.MapGrpcService<DictionaryServiceImplement>();
            //app.MapGrpcService<OrganizationServiceImplement>();
            //app.MapGrpcService<DivisionServiceImplement>();
            //app.MapGrpcService<StandardServiceImplement>();
            //app.MapGrpcService<SystemModuleServiceImplement>();

            //// School
            //app.MapGrpcService<SchoolServiceImplement>();
            //app.MapGrpcService<StudentServiceImplement>();
            //app.MapGrpcService<TeacherServiceImplement>();
            //app.MapGrpcService<ChangeServiceImplement>();

            //// Task
            //app.MapGrpcService<TaskServiceImplement>();

            // Business
            app.MapGrpcService<WxParentTerminalServiceImplement>();
            app.MapGrpcService<WxTeacherTerminalServiceImplement>();
            app.MapGrpcService<VisionScreeningCoreServiceImplement>();

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