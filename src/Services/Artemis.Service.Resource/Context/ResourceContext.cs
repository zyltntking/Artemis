using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental;
using Artemis.Data.Core.Fundamental.Design;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store.Extensions;
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
    ///     标准目录数据集
    /// </summary>
    public virtual DbSet<ArtemisStandardCatalog> StandardCatalogs { get; set; }

    /// <summary>
    ///     标准项目数据集
    /// </summary>
    public virtual DbSet<ArtemisStandardItem> StandardItems { get; set; }

    /// <summary>
    /// 系统模块数据集
    /// </summary>
    public virtual DbSet<ArtemisSystemModule> SystemModules { get; set; }

    /// <summary>
    ///     配置数据模型
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Project.Schemas.Resource);

        //SeedData(modelBuilder);
    }

    /// <summary>
    ///     种子数据，在初始化时调用
    /// </summary>
    /// <param name="modelBuilder"></param>
    private void SeedData(ModelBuilder modelBuilder)
    {
        // seed dictionary
        var dictionaries = new List<EnumerationRecord>
        {
            Enumeration.ToRecordDictionary<ArtemisClaimTypes>(),
            Enumeration.ToRecordDictionary<ChineseNation>(),
            //Enumeration.ToRecordDictionary<ChineseNationEn>(false),
            Enumeration.ToRecordDictionary<DictionaryType>(),
            Enumeration.ToRecordDictionary<EndType>(),
            Enumeration.ToRecordDictionary<Gender>(),
            Enumeration.ToRecordDictionary<IdentityPolicy>(),
            Enumeration.ToRecordDictionary<RegionLevel>(),
            Enumeration.ToRecordDictionary<SchoolLength>(),
            Enumeration.ToRecordDictionary<StudyPhase>(),
            Enumeration.ToRecordDictionary<TaskMode>(),
            Enumeration.ToRecordDictionary<TaskShip>(),
            Enumeration.ToRecordDictionary<TaskState>(),
            Enumeration.ToRecordDictionary<OrganizationType>(),
            Enumeration.ToRecordDictionary<OrganizationStatus>(),
            Enumeration.ToRecordDictionary<StandardState>(),
            Enumeration.ToRecordDictionary<StandardType>(),
            Enumeration.ToRecordDictionary<SystemModuleType>(),
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

        var orgList = new List<ArtemisOrganization>();

        var org1 = Instance.CreateInstance<ArtemisOrganization>();
        org1.Name = "红河州教体局";
        org1.Code = DesignCode.Organization("5325", 1);
        org1.DesignCode = DesignCode.Organization("5325", 1);
        org1.Type = OrganizationType.Management;
        org1.Status = OrganizationStatus.InOperation;
        org1.DivisionCode = "5325";
        orgList.Add(org1);

        var org2 = Instance.CreateInstance<ArtemisOrganization>();
        org2.Name = "蒙自市教体局";
        org2.Code = DesignCode.Organization("5325", 1, org1.Code);
        org2.DesignCode = DesignCode.Organization("5325", 1, org1.Code);
        org2.Type = OrganizationType.Management;
        org2.Status = OrganizationStatus.InOperation;
        org2.DivisionCode = "532503";
        org2.ParentId = org1.Id;
        orgList.Add(org2);

        var org3 = Instance.CreateInstance<ArtemisOrganization>();
        org3.Name = "西南联大蒙自小学";
        org3.Code = DesignCode.Organization("5325", 1, org2.Code);
        org3.DesignCode = DesignCode.Organization("5325", 1, org2.Code);
        org3.Type = OrganizationType.Functional;
        org3.Status = OrganizationStatus.InOperation;
        org3.DivisionCode = "53250301";
        org3.ParentId = org2.Id;
        orgList.Add(org3);

        var org4 = Instance.CreateInstance<ArtemisOrganization>();
        org4.Name = "蒙自市机关幼儿园";
        org4.Code = DesignCode.Organization("5325", 2, org2.Code);
        org4.DesignCode = DesignCode.Organization("5325", 2, org2.Code);
        org4.Type = OrganizationType.Functional;
        org4.Status = OrganizationStatus.InOperation;
        org4.DivisionCode = "532503101";
        org4.ParentId = org2.Id;
        orgList.Add(org4);

        //modelBuilder.Entity<ArtemisOrganization>().HasData(orgList);
    }
}