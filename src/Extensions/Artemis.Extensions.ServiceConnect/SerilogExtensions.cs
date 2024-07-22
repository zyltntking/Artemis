﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;

namespace Artemis.Extensions.ServiceConnect;

/// <summary>
///     配置扩展
/// </summary>
internal static class SerilogExtensions
{
    /// <summary>
    ///     添加Serilog配置
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    internal static void ConfigureSerilog(this IHostApplicationBuilder builder,
        string path = "serilog.Setting.json")
    {
        var serilogConfiguration = builder.Configuration.GetSection("Serilog");

        if (Path.Exists(path) && !serilogConfiguration.Exists())
        {
            builder.Configuration.AddJsonFile(path, true, true);
        }

        builder.Services.AddSerilog((services, configuration) =>
            configuration
                .ReadFrom.Configuration(builder.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.WithExceptionDetails());
    }
}