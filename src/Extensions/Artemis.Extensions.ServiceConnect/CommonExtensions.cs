using System.IO.Compression;
using Artemis.Extensions.ServiceConnect.HttpLogging;
using Artemis.Extensions.ServiceConnect.MapEndPoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
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
    /// <param name="includeDefaultService"></param>
    /// <returns></returns>
    /// <remarks>默认：OpenTelemetry, 默认健康检查，服务发现，云原生Http客户端信道，常规：httpAccessor，压缩(生产环境)，认证，授权，http日志</remarks>
    public static IHostApplicationBuilder AddServiceCommons(this IHostApplicationBuilder builder,
        bool includeDefaultService = true)
    {
        builder.ConfigureSerilog();

        if (includeDefaultService)
            builder.AddServiceDefaults();

        builder.Services.AddControllers();

        // add common services
        builder.Services.AddHttpContextAccessor();

        if (!builder.Environment.IsDevelopment())
        {
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
        }

        builder.Services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.Duration;
        });

        builder.Services.AddHttpLoggingInterceptor<ArtemisHttpLoggingInterceptor>();

        return builder;
    }

    /// <summary>
    ///     配置默认程序
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication ConfigureAppCommons(this WebApplication app)
    {
        app.UseWhen(ctx => ctx.Request.ContentType != "application/grpc", builder => { builder.UseHttpLogging(); });

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

        if (!app.Environment.IsDevelopment())
            app.UseResponseCompression();

        return app;
    }

    /// <summary>
    ///     映射常用端点
    /// </summary>
    /// <param name="app"></param>
    /// <param name="includeDefaultEndpoints">是否包含默认端点</param>
    /// <param name="useApiController"></param>
    /// <returns></returns>
    /// <remarks>默认:dev:/health 健康检查, dev:/alive 存活检查, prod:/health-detail 详细健康检查|常用：apicontroller, /route-table</remarks>
    public static WebApplication MapCommonEndpoints(this WebApplication app, bool includeDefaultEndpoints = true,
        bool useApiController = false)
    {
        if (useApiController)
            // map controllers
            app.MapControllers();

        if (includeDefaultEndpoints)
            // map default endpoints for health check
            app.MapDefaultEndpoints();

        // map route table endpoint through "/route-table"
        app.MapRouteTable();

        return app;
    }

    /// <summary>
    ///     映射常用端点
    /// </summary>
    /// <param name="app"></param>
    /// <param name="includeDefaultEndpoints">是否包含默认端点</param>
    /// <param name="useApiController">是否包含Api控制器</param>
    /// <returns></returns>
    /// <remarks>默认:dev:/health 健康检查, dev:/alive 存活检查, prod:/health-detail 详细健康检查|常用：apicontroller, /route-table，dev: /migrate</remarks>
    public static WebApplication MapCommonEndpoints<TDbContext>(this WebApplication app,
        bool includeDefaultEndpoints = true, bool useApiController = false) where TDbContext : DbContext
    {
        if (app.Environment.IsDevelopment())
            // map migration endpoint through "/migrate"
            app.MapMigrationEndpoint<TDbContext>();

        return app.MapCommonEndpoints(includeDefaultEndpoints, useApiController);
    }
}