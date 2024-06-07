using Artemis.Services.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Artemis.App.Identity;

/// <summary>
///     Identity迁移上下文工厂
/// </summary>
public class MigrationDbContextFactory : IDesignTimeDbContextFactory<ArtemisIdentityContext>
{
    #region Implementation of IDesignTimeDbContextFactory<out ArtemisIdentityContext>

    /// <summary>Creates a new instance of a derived context.</summary>
    /// <param name="args">Arguments provided by the design-time service.</param>
    /// <returns>An instance of ArtemisIdentityContext.</returns>
    public ArtemisIdentityContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<ArtemisIdentityContext>()
            .UseNpgsql(configuration.GetConnectionString("IdentityContext"), options =>
            {
                options.MigrationsHistoryTable("ArtemisIdentityHistory", "identity");

                options.MigrationsAssembly("Artemis.App.Identity");
            });


        return new ArtemisIdentityContext(builder.Options);
    }

    #endregion

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", false);

        return builder.Build();
    }
}