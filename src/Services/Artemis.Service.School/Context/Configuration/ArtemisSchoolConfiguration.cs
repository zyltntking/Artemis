using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     学校配置
/// </summary>
internal sealed class ArtemisSchoolConfiguration : ConcurrencyModelEntityConfiguration<ArtemisSchool>
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
        builder.Property(entity => entity.EstablishTime)
            .HasColumnType(DataTypeSet.DateTime);
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisSchool> builder)
    {
        // index
        builder.HasIndex(school => school.Code)
            .HasDatabaseName(IndexName(nameof(ArtemisSchool.Code)));

        builder.HasIndex(school => school.Name)
            .HasDatabaseName(IndexName(nameof(ArtemisSchool.Name)));

        builder.HasIndex(school => school.BindingTag)
            .HasDatabaseName(IndexName(nameof(ArtemisSchool.BindingTag)));

        builder.HasIndex(school => school.Type)
            .HasDatabaseName(IndexName(nameof(ArtemisSchool.Type)));

        builder.HasIndex(school => school.OrganizationCode)
            .HasDatabaseName(IndexName(nameof(ArtemisSchool.OrganizationCode)));

        builder.HasIndex(school => school.DivisionCode)
            .HasDatabaseName(IndexName(nameof(ArtemisSchool.DivisionCode)));

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