using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental;
using Artemis.Data.Core.Fundamental.Types;
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

        // seed dictionary
        var dictionaries = new List<EnumerationRecord>
        {
            Enumeration.ToRecordDictionary<ArtemisClaimTypes>(),
            Enumeration.ToRecordDictionary<IdentityPolicy>(),
            Enumeration.ToRecordDictionary<ChineseNation>(),
            //Enumeration.ToRecordDictionary<ChineseNationEn>(false),
            Enumeration.ToRecordDictionary<DictionaryType>(),
            Enumeration.ToRecordDictionary<EndType>(),
            Enumeration.ToRecordDictionary<Gender>(),
            Enumeration.ToRecordDictionary<RegionLevel>(),
            Enumeration.ToRecordDictionary<TaskMode>(),
            Enumeration.ToRecordDictionary<TaskShip>()
        };

        foreach (var dictionary in dictionaries)
        {
            var dataDictionary = Generator.CreateInstance<ArtemisDataDictionary>();

            dataDictionary.Id = Guid.NewGuid();
            dataDictionary.Name = dictionary.TypeName;
            dataDictionary.Code = dictionary.TypeName;
            dataDictionary.Valid = dictionary.Valid;
            dataDictionary.Type = DictionaryType.Public;
            dataDictionary.Description = dictionary.Description;

            modelBuilder.Entity<ArtemisDataDictionary>().HasData(dataDictionary);

            var dictionaryItems = dictionary.Records.Select(item =>
            {
                var dataDictionaryItem = Generator.CreateInstance<ArtemisDataDictionaryItem>();

                dataDictionaryItem.Id = Guid.NewGuid();
                dataDictionaryItem.DataDictionaryId = dataDictionary.Id;
                dataDictionaryItem.Key = item.ItemKey;
                dataDictionaryItem.Value = item.ItemValue;
                dataDictionaryItem.Description = item.Description;

                return dataDictionaryItem;
            });

            modelBuilder.Entity<ArtemisDataDictionaryItem>().HasData(dictionaryItems);
        }
    }
}