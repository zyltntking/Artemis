using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Artemis.Extensions.Web.Serilog;

/// <summary>
/// 常规补充日志中间件
/// </summary>
internal class SupplementalLogMiddleware
{
    private RequestDelegate Next { get; }

    private ILogger Logger { get; }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="next">请求代理</param>
    /// <param name="logger"></param>
    public SupplementalLogMiddleware(RequestDelegate next, ILogger<SupplementalLogMiddleware> logger)
    {
        Next = next;
        Logger = logger;
    }

    /// <summary>
    /// 调用逻辑
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress;

        Logger.LogInformation($"客户端IP：{ip}");
        await Next(context);
    }
}