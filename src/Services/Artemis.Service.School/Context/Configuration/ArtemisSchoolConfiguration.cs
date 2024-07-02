using Artemis.Data.Store.Configuration;
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
    protected override string TableName => nameof(ArtemisSchool);

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
            .HasConstraintName(ForeignKeyName(nameof(ArtemisClass), nameof(ArtemisSchool)))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}