using System.Text.Json.Serialization;
using Artemis.App.Logic.IdentityLogic;
using Artemis.Extensions.Web.Builder;
using Artemis.Extensions.Web.Middleware;
using Artemis.Extensions.Web.OpenApi;
using Artemis.Extensions.Web.Serilog;

namespace Artemis.App.IdentityApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var docConfig = new DocumentConfig();

            LogHost.CreateWebApp(args, builder =>
            {
                // Add services to the container.
                var connectionString = builder.Configuration
                    .GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

                builder.Services.AddIdentityService(new IdentityLogicOptions
                {
                    Connection = connectionString,
                    AssemblyName = "Artemis.App.IdentityApplication"
                });

                builder.Services.AddGrpc();
                builder.Services.AddGrpcReflection();

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

                builder.Services.Configure<DomainOptions>(options =>
                {
                    options.DomainName = "Identity";
                });
                
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddScoped<ServiceDomainMiddleware>();
                builder.Services.AddScoped<ExceptionResultMiddleware>();

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

                app.UseMiddleware<ServiceDomainMiddleware>();
                app.UseMiddleware<ExceptionResultMiddleware>();

                app.MapRazorPages();

                app.MapControllers();

                app.MapApiRouteTable();

                if (app.Environment.IsDevelopment())
                {
                    app.MapGrpcReflectionService();
                }
            });
        }
    }
}