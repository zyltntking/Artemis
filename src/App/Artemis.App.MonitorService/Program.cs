using Artemis.Core.Monitor.Store;
using Artemis.Data.Store;
using Microsoft.EntityFrameworkCore;

namespace Artemis.App.MonitorService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        MonitorSetting.DbType = DbType.PostgreSql;

        builder.Services.AddDbContext<MonitorDbContext>(options =>
        {
            options.UseNpgsql("Host=localhost;Database=Monitor;Username=postgres;Password=123456",
                b => b.MigrationsAssembly("Artemis.App.MonitorService"));
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

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}