using Artemis.Service.Identity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Artemis.App.Identity;

/// <summary>
///     Identity迁移上下文工厂
/// </summary>
public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityContext>
{
    #region Implementation of IDesignTimeDbContextFactory<out ArtemisIdentityContext>

    /// <summary>Creates a new instance of a derived context.</summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of ArtemisIdentityContext.</returns>
    public IdentityContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<IdentityContext>()
            .UseNpgsql(configuration.GetConnectionString("IdentityContext"), options =>
            {
                options.MigrationsHistoryTable("IdentityDbHistory", "identity");
                options.MigrationsAssembly("Artemis.App.Identity");
            });


        return new IdentityContext(builder.Options);
    }

    #endregion

    /// <summary>
    ///     创建配置
    /// </summary>
    /// <returns></returns>
    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false)
            .AddJsonFile("appsettings.Development.json", false);

        return builder.Build();
    }
}