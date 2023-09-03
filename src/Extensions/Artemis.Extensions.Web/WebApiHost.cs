using System.Text.Json.Serialization;
using Artemis.Extensions.Web.OpenApi;
using Artemis.Extensions.Web.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Artemis.Extensions.Web;

/// <summary>
///     WebApi主机
/// </summary>
public static class WebApiHost
{
    /// <summary>
    ///     创建主机
    /// </summary>
    /// <param name="args">应用控制参数</param>
    /// <param name="buildAction">应用创建行为</param>
    /// <param name="appAction">应用行为</param>
    public static void CreateHost(string[] args, Action<WebApplicationBuilder>? buildAction = null,
        Action<WebApplication>? appAction = null)
    {
        var docConfig = new DocumentConfig();

        LogHost.CreateWebApp(args, builder =>
        {
            buildAction?.Invoke(builder);

            builder.Services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.AddOpenApiDoc(docConfig);
        }, app =>
        {
            appAction?.Invoke(app);

            app.UseOpenApiDoc(docConfig);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // app.UseDatabaseErrorPage();
                // app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // app.UseStaticFiles();
            // app.UseCookiePolicy();

            // app.UseRouting();
            // app.UseRateLimiter();
            // app.UseRequestLocalization();
            // app.UseCors();

            //app.UseAuthentication();
            app.UseAuthorization();
            // app.UseSession();
            // app.UseResponseCompression();
            // app.UseResponseCaching();

            app.MapControllers();
        });
    }
}