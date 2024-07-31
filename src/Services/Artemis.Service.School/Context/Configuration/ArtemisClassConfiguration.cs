using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
    ///     计算年级名称
    /// </summary>
    /// <param name="establishTime"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public double CountGradeName(DateTime establishTime)
    {
        throw new NotSupportedException();
    }

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