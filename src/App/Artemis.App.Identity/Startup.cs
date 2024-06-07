using Artemis.App.Identity.Services;
using Artemis.Extensions.Rpc;
using Artemis.Extensions.Web.Identity;
using Artemis.Extensions.Web.Serilog;
using Artemis.Extensions.Web.Validators;
using Artemis.Services.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Artemis.App.Identity;

/// <summary>
///     启动器配置
/// </summary>
public class Startup : IWebAppStartup
{
    #region Implementation of IWebAppStartup

    /// <summary>
    ///     配置服务
    /// </summary>
    /// <param name="builder">程序集</param>
    public void ConfigureBuilder(WebApplicationBuilder builder)
    {
        // Add services to the container.
        var pgsqlConnectionString = builder.Configuration.GetConnectionString("IdentityContext") ??
                                    throw new InvalidOperationException(
                                        "ContextConnection string 'Identity' not found.");

        builder.Services.AddArtemisIdentityService(new IdentityServiceOptions
        {
            EnableCache = true,
            RegisterDbAction = dbBuilder =>
            {
                dbBuilder.UseNpgsql(pgsqlConnectionString, npgsqlOption =>
                {
                    npgsqlOption.MigrationsHistoryTable("ArtemisIdentityHistory", "identity");

                    npgsqlOption.MigrationsAssembly("Artemis.App.Identity");

                    npgsqlOption.EnableRetryOnFailure(3);

                    npgsqlOption.CommandTimeout(30);
                });
            },
            IdentityOptionsAction = options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredUniqueChars = 1;
            }
        }, builder.Environment.IsDevelopment());

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
        });

        builder.Services.AddGrpc(options =>
        {
            options.EnableDetailedErrors = true;
            options.Interceptors.Add<MessageValidationInterceptor>();
            options.Interceptors.Add<AddInLogInterceptor>();
            options.Interceptors.Add<FriendlyExceptionInterceptor>();
        }).AddJsonTranscoding();
        builder.Services.AddGrpcReflection();
        builder.Services.AddGrpcSwagger();
        builder.Services.AddSwaggerGen(config =>
        {
            config.SwaggerDoc("v1",
                new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });

            var protosDocPath = Path.Combine(AppContext.BaseDirectory, "Artemis.Protos.Identity.xml");
            config.IncludeXmlComments(protosDocPath);
            config.IncludeGrpcXmlComments(protosDocPath, true);

            var servicesDocPath = Path.Combine(AppContext.BaseDirectory, "Artemis.App.Identity.xml");
            config.IncludeXmlComments(servicesDocPath);
            config.IncludeGrpcXmlComments(servicesDocPath, true);

            config.OperationFilter<RemoveDefaultResponse>();
            config.DocumentFilter<RemoveDefaultSchemas>();
            config.OperationFilter<AddIdentityToken>();
            //config.OperationFilter<MarkFieldFeature>();
        });

        builder.Services.AddValidators();

        builder.Services.AddArtemisAuthorization<RpcAuthorizationResultTransformer>(new ArtemisIdentityOptions
        {
            EnableAdvancedPolicy = false,
            HeaderIdentityTokenKey = Constants.HeaderIdentityTokenKey,
            CacheIdentityTokenPrefix = Constants.CacheIdentityTokenPrefix,
            UserMapTokenPrefix = Constants.UserMapTokenPrefix,
            CacheIdentityTokenExpire = 604800, //7 days
            EnableMultiEnd = false
        });

        builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });


        //builder.Services.AddControllers();
        //builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSwaggerGen(options =>
        //{
        //    options.SwaggerDoc("v1",
        //        new OpenApiInfo { Title = "gRPC transcoding", Version = "v1" });
        //});

        //builder.Services.AddCodeFirstGrpc(options =>
        //{
        //    options.EnableDetailedErrors = true;
        //    options.Interceptors.Add<AddInLogInterceptor>();
        //});

        //if (builder.Environment.IsDevelopment())
        //    builder.Services.AddCodeFirstGrpcReflection();
    }

    /// <summary>
    ///     应用配置
    /// </summary>
    /// <param name="app"></param>
    public void ConfigureApplication(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseRouting();

        //app.UseSwagger(options => { });
        //app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseResponseCompression();

        app.UseSwagger();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });

        app.UseReDoc(c =>
        {
            c.RoutePrefix = "api-docs";
            c.SpecUrl("/swagger/v1/swagger.json");
            c.DocumentTitle = "Artemis Identity API";
        });

        app.MapGrpcService<AccountService>();
        app.MapGrpcService<UserService>();

        //app.MapDefaultArtemisIdentityGrpcServices();

        if (app.Environment.IsDevelopment())
        {
            app.MapGet("api-route-table", (IEnumerable<EndpointDataSource> endpointSources, HttpContext context) =>
            {
                var ends = endpointSources.SelectMany(es => es.Endpoints);
            });

            app.MapGrpcReflectionService();
        }
        //app.MapApiRouteTable();
        //app.MapCodeFirstGrpcReflectionService();
    }

    #endregion
}