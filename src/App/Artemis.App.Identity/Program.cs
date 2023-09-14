using System.Text.Json.Serialization;
using Artemis.Extensions.Web.Builder;
using Artemis.Extensions.Web.Middleware;
using Artemis.Extensions.Web.OpenApi;
using Artemis.Extensions.Web.Serilog;
using Artemis.Services.Identity;

namespace Artemis.App.Identity;

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
            // Add services to the container.
            var connectionString = builder.Configuration
                                       .GetConnectionString("Identity") ??
                                   throw new InvalidOperationException("Connection string 'Identity' not found.");

            builder.Services.AddIdentityService(new IdentityServiceOptions
            {
                Connection = connectionString,
                AssemblyName = "Artemis.App.Identity"
            }, builder.Environment.IsDevelopment());

            //builder.Services.AddGrpc();
            //builder.Services.AddCodeFirstGrpc();
            //builder.Services.AddGrpcReflection();
            //builder.Services.AddCodeFirstGrpcReflection();

            builder.Services.AddRazorPages();

            builder.Services
                .AddControllers()
                .AddMvcOptions(options =>
                {
                    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                        name => $"×Ö¶Î:{name}ÊÇ±ØÒª×Ö¶Î.");
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            builder.Services.AddArtemisMiddleWares(options => { options.ServiceDomain.DomainName = "Identity"; });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.AddOpenApiDoc(docConfig);
        }, app =>
        {
            app.UseOpenApiDoc(docConfig);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseArtemisMiddleWares();

            app.MapRazorPages();

            app.MapControllers();

            //app.MapGrpcService<GreeterService>();
            //app.MapGrpcService<SampleService>();

            app.MapApiRouteTable();

            if (app.Environment.IsDevelopment())
            {
                //app.MapGrpcReflectionService();
                //app.MapCodeFirstGrpcReflectionService();
            }
        });
    }
}