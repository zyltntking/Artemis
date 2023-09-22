using System.Text.Json.Serialization;
using Artemis.App.Identity.Interceptors;
using Artemis.App.Identity.Services;
using Artemis.Extensions.Web.Builder;
using Artemis.Extensions.Web.Middleware;
using Artemis.Extensions.Web.OpenApi;
using Artemis.Extensions.Web.Serilog;
using Artemis.Services.Identity;
using ProtoBuf.Grpc.Server;

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
                                       .GetConnectionString("IdentityContext") ??
                                   throw new InvalidOperationException(
                                       "ContextConnection string 'Identity' not found.");

            builder.Services.AddIdentityService(new IdentityServiceOptions
            {
                ContextConnection = connectionString,
                RedisCacheConnection = builder.Configuration.GetConnectionString("RedisCache"),
                AssemblyName = "Artemis.App.Identity"
            }, builder.Environment.IsDevelopment());

            builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });

            builder.Services.AddRazorPages();

            builder.Services
                .AddControllers(option =>
                {
                    option.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                    option.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
                })
                .AddMvcOptions(options =>
                {
                    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                        name => $"×Ö¶Î:{name}ÊÇ±ØÒª×Ö¶Î.");
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            //builder.Services.AddGrpc();
            //builder.Services.AddGrpcReflection();
            builder.Services.AddCodeFirstGrpc(options =>
            {
                options.EnableDetailedErrors = true;
                options.Interceptors.Add<TokenInterceptor>();
            });

            builder.Services.AddCodeFirstGrpcReflection();

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

            app.UseResponseCompression();

            app.MapRazorPages();

            app.MapControllers();

            app.MapGrpcService<RoleService>();

            app.MapApiRouteTable();

            if (app.Environment.IsDevelopment())
                //app.MapGrpcReflectionService();
                app.MapCodeFirstGrpcReflectionService();
        });
    }
}