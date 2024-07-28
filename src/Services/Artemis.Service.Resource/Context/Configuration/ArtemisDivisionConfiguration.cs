using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Resource.Context.Configuration;

/// <summary>
///     行政区划配置
/// </summary>
internal sealed class ArtemisDivisionConfiguration : ConcurrencyModelEntityConfiguration<ArtemisDivision>
{
    #region Overrides of BaseEntityConfiguration<ArtemisDivision,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "行政区划数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisDivision).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisDivision> builder)
    {
        builder.Property<string>("ProvinceCode")
            .HasComputedColumnSql(@"substring(""Code"", 1, 2)", true)
            .HasMaxLength(4)
            .HasComment("省级行政区划编码");

        builder.Property<string>("PrefectureCode")
            .HasComputedColumnSql(@"substring(""Code"", 1, 4)", true)
            .HasMaxLength(6)
            .HasComment("地级行政区划编码");

        builder.Property<string>("CountyCode")
            .HasComputedColumnSql(@"substring(""Code"", 1, 6)", true)
            .HasMaxLength(8)
            .HasComment("县级行政区划编码");

        builder.Property<string>("TownshipCode")
            .HasComputedColumnSql(@"substring(""Code"", 1, 9)", true)
            .HasMaxLength(11)
            .HasComment("乡级行政区划编码");
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisDivision> builder)
    {
        // Shadow Properties
        builder.HasIndex("ProvinceCode")
            .HasDatabaseName(IndexName("ProvinceCode"));

        builder.HasIndex("PrefectureCode")
            .HasDatabaseName(IndexName("PrefectureCode"));

        builder.HasIndex("CountyCode")
            .HasDatabaseName(IndexName("CountyCode"));

        builder.HasIndex("TownshipCode")
            .HasDatabaseName(IndexName("TownshipCode"));

        // Each Division can have many Children Division
        builder.HasMany(division => division.Children)
            .WithOne(child => child.Parent)
            .HasForeignKey(child => child.ParentId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisDivision).TableName(),
                nameof(ArtemisDivision).TableName()))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}