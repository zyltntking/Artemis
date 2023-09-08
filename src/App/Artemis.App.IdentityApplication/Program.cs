using System.Text.Json.Serialization;
using Artemis.App.Logic.IdentityLogic;
using Artemis.Extensions.Web.Builder;
using Artemis.Extensions.Web.Middleware;
using Artemis.Extensions.Web.OpenApi;
using Artemis.Extensions.Web.Serilog;
using Microsoft.AspNetCore.Identity;

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

                builder.Services.AddRazorPages();

                builder.Services
                    .AddControllers()
                    .AddMvcOptions(options =>
                    {
                        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                            name => $"�ֶ�:{name}�Ǳ�Ҫ�ֶ�.");
                    })
                    .AddJsonOptions(options =>
                    {
                        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    });

                builder.Services.Configure<DomainOptions>(options =>
                {
                    options.DomainName = "Identity";
                });

                builder.Services.Configure<IdentityOptions>(options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = false;
                });

                builder.Services.ConfigureApplicationCookie(options =>
                {
                    // Cookie settings
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                    options.LoginPath = "/Identity/Account/Login";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                    options.SlidingExpiration = true;
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
            });
        }
    }
}