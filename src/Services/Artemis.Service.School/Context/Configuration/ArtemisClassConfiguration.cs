using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     班级配置
/// </summary>
internal sealed class ArtemisClassConfiguration : ConcurrencyPartitionEntityConfiguration<ArtemisClass>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisClass>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "班级数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisClass).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisClass> builder)
    {
        // Each Class can have one HeadTeacher
        builder.HasOne(iClass => iClass.HeadTeacher)
            .WithOne(teacher => teacher.HeadTeacherClass)
            .HasForeignKey<ArtemisClass>(iClass => iClass.HeadTeacherId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisTeacher).TableName(),
                nameof(ArtemisClass).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.SetNull);
    }

    #endregion
}