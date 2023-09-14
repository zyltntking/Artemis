using Microsoft.AspNetCore.Builder;
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
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("Starting web application");

            var builder = WebApplication.CreateBuilder(args);

            buildAction(builder);

            builder.Host.UseSerilog();

            var app = builder.Build();

            appAction(app);

            app.Run();
        }
        catch (Exception exception)
        {
            if (exception is not HostAbortedException) Log.Fatal(exception, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}