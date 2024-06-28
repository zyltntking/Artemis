using System.IO.Compression;
using Artemis.Extensions.ServiceConnect.Authorization;
using Artemis.Extensions.ServiceConnect.HttpLogging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.ResponseCompression;
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
    public static void AddServiceCommons(this IHostApplicationBuilder builder)
    {
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

        builder.Services.AddAuthentication();
        //配置授权
        builder.ConfigureAuthorization();

        builder.Services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.Duration;
        });

        builder.Services.AddHttpLoggingInterceptor<ArtemisHttpLoggingInterceptor>();
    }

    /// <summary>
    ///     配置默认程序
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static void ConfigureAppCommon(this WebApplication app)
    {
        app.UseHttpLogging();

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

        if (!app.Environment.IsDevelopment()) app.UseResponseCompression();
    }
}