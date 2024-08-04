using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     学生配置
/// </summary>
internal sealed class ArtemisStudentConfiguration : ConcurrencyModelEntityConfiguration<ArtemisStudent>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisStudent>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "学生数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisStudent).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisStudent> builder)
    {
        builder.Property(student => student.EnrollmentDate)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(student => student.Birthday)
            .HasColumnType(DataTypeSet.DateTime);
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisStudent> builder)
    {
        // index
        builder.HasIndex(student => student.Name)
            .HasDatabaseName(IndexName(nameof(ArtemisStudent.Name)));

        builder.HasIndex(student => student.Gender)
            .HasDatabaseName(IndexName(nameof(ArtemisStudent.Gender)));

        builder.HasIndex(student => student.Nation)
            .HasDatabaseName(IndexName(nameof(ArtemisStudent.Nation)));

        builder.HasIndex(student => student.StudentNumber)
            .HasDatabaseName(IndexName(nameof(ArtemisStudent.StudentNumber)));
    }

    #endregion
}