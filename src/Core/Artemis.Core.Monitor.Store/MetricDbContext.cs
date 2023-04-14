using Artemis.Core.Monitor.Store.Entities;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Core.Monitor.Store;

/// <summary>
///     监控数据上下文
/// </summary>
public class MetricDbContext : DbContext
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="options"></param>
    public MetricDbContext(DbContextOptions<MetricDbContext> options) : base(options)
    {
    }

    /// <summary>
    ///    元数据组
    /// </summary>
    public DbSet<MetadataGroup> MetadataGroups { get; set; }

}