using Artemis.Core.Monitor.Store.Entities;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Core.Monitor.Store;

/// <summary>
///     监控数据上下文
/// </summary>
public class MonitorDbContext : DbContext
{
    /// <summary>
    ///     构造
    /// </summary>
    /// <param name="options"></param>
    public MonitorDbContext(DbContextOptions<MonitorDbContext> options) : base(options)
    {
    }

    /// <summary>
    ///    元数据组
    /// </summary>
    public DbSet<MetadataGroup> MetadataGroups { get; set; }

    /// <summary>
    /// 元数据
    /// </summary>
    public DbSet<MetadataItem> MetadataItems { get; set; }

}