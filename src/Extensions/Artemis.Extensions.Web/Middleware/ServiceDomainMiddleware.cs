using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Artemis.Extensions.Web.Middleware;

/// <summary>
///     服务域中间件
/// </summary>
public class ServiceDomainMiddleware : IMiddleware
{
    /// <summary>
    ///     服务域标识
    /// </summary>
    private const string DomainKey = SharedKeys.DomainKey;

    /// <summary>
    ///     中间件构造
    /// </summary>
    /// <param name="options">域</param>
    /// <param name="logger">日志依赖</param>
    public ServiceDomainMiddleware(IOptions<ArtemisMiddlewareOptions> options, ILogger<ServiceDomainMiddleware> logger)
    {
        Options = options.Value;
        Logger = logger;
    }

    #region Implementation of IMiddleware

    /// <summary>Request handling method.</summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Http.HttpContext" /> for the current request.</param>
    /// <param name="next">The delegate representing the remaining middleware in the request pipeline.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that represents the execution of this middleware.</returns>
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        context.Items.Add(DomainKey, Options.ServiceDomain.DomainName);

        return next(context);
    }

    #endregion

    /// <summary>
    ///     域名称
    /// </summary>
    private ArtemisMiddlewareOptions Options { get; }

    /// <summary>
    ///     日志
    /// </summary>
    private ILogger<ServiceDomainMiddleware> Logger { get; }
}