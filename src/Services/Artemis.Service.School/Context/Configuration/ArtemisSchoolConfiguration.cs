using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     学校配置
/// </summary>
internal sealed class ArtemisSchoolConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisSchool>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisSchool>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "学校数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisSchool).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisSchool> builder)
    {
        // Shadow Properties
        builder.Property<string>("ProvinceCode")
            .HasComputedColumnSql(@"substring(""OrganizationCode"", 1, 2)", true)
            .HasMaxLength(4)
            .HasComment("省级行政区划编码");

        builder.Property<string>("PrefectureCode")
            .HasComputedColumnSql(@"substring(""OrganizationCode"", 1, 4)", true)
            .HasMaxLength(6)
            .HasComment("地级行政区划编码");

        builder.Property<string>("CountyCode")
            .HasComputedColumnSql(@"substring(""OrganizationCode"", 1, 6)", true)
            .HasMaxLength(8)
            .HasComment("县级行政区划编码");

        builder.Property<string>("TownshipCode")
            .HasComputedColumnSql(@"substring(""OrganizationCode"", 1, 9)", true)
            .HasMaxLength(11)
            .HasComment("乡级行政区划编码");
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisSchool> builder)
    {
        // Each School can have many Classes
        builder.HasMany(school => school.Classes)
            .WithOne(iClass => iClass.School)
            .HasForeignKey(iClass => iClass.SchoolId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisClass).TableName(),
                nameof(ArtemisSchool).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}