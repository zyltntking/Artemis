using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Artemis.Extensions.Web.Serilog;

/// <summary>
///     日志扩展
/// </summary>
public static class LogHost
{
    /// <summary>
    ///     创建Web应用
    /// </summary>
    /// <param name="args">应用控制参数</param>
    /// <param name="buildAction">应用创建行为</param>
    /// <param name="appAction">应用行为</param>
    public static void CreateWebApp(string[] args, Action<WebApplicationBuilder> buildAction,
        Action<WebApplication> appAction)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("serilog.json", false, true)
            .Build();


        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        Log.Information("启动Web应用");

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            buildAction(builder);

            builder.Host.UseSerilog();

            var app = builder.Build();

            appAction(app);

            app.UseSerilogRequestLogging();
            app.UseMiddleware<SupplementalLogMiddleware>();

            app.Run();
        }
        catch (Exception exception)
        {
            if (exception is HostAbortedException)
                Log.Information("启动中止，若当前进程为迁移进程，可忽略这条信息");
            else
                Log.Fatal(exception, "异常停机");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    ///     创建Web应用
    /// </summary>
    /// <typeparam name="TStartup"></typeparam>
    /// <param name="args"></param>
    public static void CreateWebApp<TStartup>(string[] args) where TStartup : IWebAppStartup, new()
    {
        var startup = new TStartup();

        CreateWebApp(args, startup.ConfigureBuilder, startup.ConfigureApplication);
    }
}