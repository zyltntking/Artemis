using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Context;

/// <summary>
///     资源上下文
/// </summary>
public class ResourceContext : DbContext
{
    /// <summary>
    ///     初始化资源上下文
    /// </summary>
    /// <param name="options">数据库上下文选项</param>
    public ResourceContext(DbContextOptions<ResourceContext> options) : base(options)
    {
    }

    /// <summary>
    ///     组织机构数据集
    /// </summary>
    public virtual DbSet<ArtemisOrganization> Organizations { get; set; }

    /// <summary>
    ///     行政区划数据集
    /// </summary>
    public virtual DbSet<ArtemisDivision> Divisions { get; set; }

    /// <summary>
    ///     设备数据集
    /// </summary>
    public virtual DbSet<ArtemisDevice> Devices { get; set; }

    /// <summary>
    ///     数据字典数据集
    /// </summary>
    public virtual DbSet<ArtemisDataDictionary> DataDictionaries { get; set; }

    /// <summary>
    ///     数据字典项数据集
    /// </summary>
    public virtual DbSet<ArtemisDataDictionaryItem> DataDictionaryItems { get; set; }

    /// <summary>
    ///     配置数据模型
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Project.Schemas.Resource);
    }
}