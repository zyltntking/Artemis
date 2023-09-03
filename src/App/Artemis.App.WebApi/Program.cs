using Artemis.Extensions.Web.OpenApi;
using Artemis.Extensions.Web.Serilog;
using System.Text.Json.Serialization;

namespace Artemis.App.WebApi;

/// <summary>
///     Program
/// </summary>
public static class Program
{
    /// <summary>
    ///     Main
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        var docConfig = new DocumentConfig();

        LogHost.CreateWebApp(args, builder =>
        {
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
                options.InstanceName = "ArtemisInstance";
            });

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

            app.Run();
        });
    }
}