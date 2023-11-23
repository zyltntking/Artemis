using Microsoft.AspNetCore.Builder;

namespace Artemis.Extensions.Web.Serilog;

/// <summary>
/// 启动类模板
/// </summary>
public interface IWebAppStartup
{
    /// <summary>
    /// 配置服务
    /// </summary>
    /// <param name="builder">程序集</param>
    void ConfigureBuilder(WebApplicationBuilder builder);

    /// <summary>
    /// 应用配置
    /// </summary>
    /// <param name="app"></param>
    void ConfigureApplication(WebApplication app);
}