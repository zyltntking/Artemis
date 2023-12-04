using Artemis.App.BroadcastApi.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Artemis.App.BroadcastApi;

/// <summary>
///     Program
/// </summary>
public static class Program
{
    /// <summary>
    ///     Main
    /// </summary>
    /// <param name="args"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.File(
                "logs/log.log",
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 30)
            .CreateLogger();

        try
        {
            Log.Information("启动Web应用");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("BroadcastContext") ??
                                   throw new InvalidOperationException(
                                       "ContextConnection string 'Identity' not found.");

            builder.Services.AddDbContextPool<BroadcastContext>(dbOptions =>
            {
                dbOptions.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsHistoryTable("ArtemisBroadcastHistory");

                    npgsqlOptions.MigrationsAssembly("Artemis.App.BroadcastApi");

                    npgsqlOptions.EnableRetryOnFailure(3);
                });
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }
        catch (Exception exception)
        {
            if (exception is not HostAbortedException)
                Log.Fatal(exception, "异常停机");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}