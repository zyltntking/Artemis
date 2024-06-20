using Artemis.Extensions.ServiceConnect;
using Artemis.Extensions.ServiceConnect.Interceptors;
using Artemis.Service.Identity.Context;
using Artemis.Service.Identity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Artemis.App.Identity;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddServiceDefaults();
        builder.AddServiceCommons();

        // Add services to the container.
        builder.AddRedisComponent("RedisInstance");

        builder.AddNpgsqlDbContext<IdentityContext>("ArtemisDb", configureDbContextOptions: config =>
        {
            config.UseNpgsql(options =>
                {
                    options.MigrationsHistoryTable("IdentityDbHistory", "identity");
                    options.MigrationsAssembly("Artemis.App.Identity");
                })
                .EnableServiceProviderCaching()
                .EnableDetailedErrors(builder.Environment.IsDevelopment())
                .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
                .LogTo(Console.WriteLine, LogLevel.Information);
        });

        //builder.Services.AddAuthorization();

        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<MessageValidator>();
            options.Interceptors.Add<FriendlyException>();
        }).AddJsonTranscoding();
        builder.Services.AddGrpcReflection();
        builder.Services.AddGrpcSwagger();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
        });

        var app = builder.Build();

        app.ConfigureAppCommon();

        // todo ƒ¨»œ∂Àµ„≥È¿Î∑÷Œˆ
        //app.MapDefaultEndpoints();

        app.UseGrpc();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<UserService>();


        //app.MapGet("/", (IdentityContext context, IDistributedCache cache) =>
        //{
        //    cache.SetString("foo", "bar");

        //    var cc = cache.GetString("foo");

        //    try
        //    {
        //        context.Database.Migrate();
        //    }
        //    catch
        //    {
        //        return "Failed!";
        //    }

        //    return "Success!";
        //});

        app.Run();
    }
}