using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artemis.Extensions.ServiceConnect;

/// <summary>
///     常用配置扩展
/// </summary>
public static class CommonExtensions
{
    /// <summary>
    ///     添加常用服务配置
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddServiceCommons(this IHostApplicationBuilder builder)
    {
        // add common services
        builder.Services.AddHttpContextAccessor();

        builder.Services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults
                .MimeTypes
                .Concat([
                    "application/octet-stream"
                ]);
        });

        builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        builder.Services.Configure<GzipCompressionProviderOptions>(options =>
        {
            options.Level = CompressionLevel.Fastest;
        });

        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        return builder;
    }

    /// <summary>
    ///     配置默认程序
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication ConfigureAppCommon(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseResponseCompression();

        return app;
    }

    /// <summary>
    ///     映射迁移端点
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="app"></param>
    /// <param name="pattern"></param>
    /// <returns></returns>
    public static WebApplication MapMigrationEndpoint<TDbContext>(this WebApplication app, string pattern = "/migrate")
        where TDbContext : DbContext
    {
        app.MapGet(pattern, (TDbContext context) =>
        {
            try
            {
                context.Database.Migrate();
            }
            catch
            {
                return "Failed!";
            }

            return "Success!";
        });

        return app;
    }
}