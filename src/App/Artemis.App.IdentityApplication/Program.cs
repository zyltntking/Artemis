using System.Collections;
using System.Collections.Generic;
using System.Text;
using Artemis.App.IdentityApplication.Data;
using Artemis.Extensions.Web.Builder;
using Artemis.Extensions.Web.Middleware;
using Artemis.Extensions.Web.OpenApi;
using Artemis.Extensions.Web.Serilog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

                builder.Services
                    .AddDbContext<ArtemisIdentityDbContext>(options =>
                    {
                        options.UseNpgsql(connectionString, npgsqlOption =>
                        {
                            npgsqlOption.MigrationsHistoryTable("ArtemisIdentityHistory", "identity");
                        }).UseLazyLoadingProxies();
                    })
                    .AddDefaultIdentity<ArtemisIdentityUser>(options =>
                    {
                        options.SignIn.RequireConfirmedAccount = true;
                    })
                    .AddEntityFrameworkStores<ArtemisIdentityDbContext>();

                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services.AddRazorPages();

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

                builder.Services.AddTransient<ExceptionResultMiddleware>();

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

                app.UseMiddleware<ExceptionResultMiddleware>();

                app.MapRazorPages();

                app.MapControllers();

                app.MapRouteTable();
            });
        }
    }
}