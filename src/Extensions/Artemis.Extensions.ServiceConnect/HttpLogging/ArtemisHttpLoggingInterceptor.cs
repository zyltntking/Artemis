using Microsoft.AspNetCore.HttpLogging;

namespace Artemis.Extensions.ServiceConnect.HttpLogging;

/// <summary>
/// Http日志拦截器
/// </summary>
internal sealed class ArtemisHttpLoggingInterceptor : IHttpLoggingInterceptor
{
    #region Implementation of IHttpLoggingInterceptor

    /// <summary>
    /// A callback to customize the logging of the request and response.
    /// </summary>
    /// <remarks>
    /// This is called when the request is first received and can be used to configure both request and response options. All settings will carry over to
    /// <see cref="M:Microsoft.AspNetCore.HttpLogging.IHttpLoggingInterceptor.OnResponseAsync(Microsoft.AspNetCore.HttpLogging.HttpLoggingInterceptorContext)" /> except the <see cref="P:Microsoft.AspNetCore.HttpLogging.HttpLoggingInterceptorContext.Parameters" />
    /// will be cleared after logging the request. <see cref="P:Microsoft.AspNetCore.HttpLogging.HttpLoggingInterceptorContext.LoggingFields" /> may be changed per request to control the logging behavior.
    /// If no request fields are enabled, and the <see cref="P:Microsoft.AspNetCore.HttpLogging.HttpLoggingInterceptorContext.Parameters" /> collection is empty, no request logging will occur.
    /// If <see cref="P:Microsoft.AspNetCore.HttpLogging.HttpLoggingOptions.CombineLogs" /> is enabled then <see cref="P:Microsoft.AspNetCore.HttpLogging.HttpLoggingInterceptorContext.Parameters" /> will carry over from the request to response
    /// and be logged together.
    /// </remarks>
    public ValueTask OnRequestAsync(HttpLoggingInterceptorContext logContext)
    {
        if (logContext.HttpContext.Request.Path.Value?.StartsWith("/api/") == true)
        {
            logContext.LoggingFields = HttpLoggingFields.All;
        }

        return ValueTask.CompletedTask;
    }

    /// <summary>A callback to customize the logging of the response.</summary>
    /// <remarks>
    /// This is called when the first write to the response happens, or the response ends without a write, just before anything is sent to the client. Settings are carried
    /// over from <see cref="M:Microsoft.AspNetCore.HttpLogging.IHttpLoggingInterceptor.OnRequestAsync(Microsoft.AspNetCore.HttpLogging.HttpLoggingInterceptorContext)" /> (except the <see cref="P:Microsoft.AspNetCore.HttpLogging.HttpLoggingInterceptorContext.Parameters" />) and response settings may
    /// still be modified. Changes to request settings will have no effect. If no response fields are enabled, and the <see cref="P:Microsoft.AspNetCore.HttpLogging.HttpLoggingInterceptorContext.Parameters" />
    /// collection is empty, no response logging will occur.
    /// If <see cref="P:Microsoft.AspNetCore.HttpLogging.HttpLoggingOptions.CombineLogs" /> is enabled then <see cref="P:Microsoft.AspNetCore.HttpLogging.HttpLoggingInterceptorContext.Parameters" /> will carry over from the request to response
    /// and be logged together. <see cref="F:Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestBody" /> and <see cref="F:Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.ResponseBody" />  can also be disabled in OnResponseAsync to prevent
    /// logging any buffered body data.
    /// </remarks>
    public ValueTask OnResponseAsync(HttpLoggingInterceptorContext logContext)
    {
        return ValueTask.CompletedTask;
    }

    #endregion
}