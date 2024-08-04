using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     教师配置
/// </summary>
internal sealed class ArtemisTeacherConfiguration : ConcurrencyModelEntityConfiguration<ArtemisTeacher>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisTeacher>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "教师数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisTeacher).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisTeacher> builder)
    {
        builder.Property(teacher => teacher.EntryTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(teacher => teacher.Birthday)
            .HasColumnType(DataTypeSet.DateTime);
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisTeacher> builder)
    {
        // index
        builder.HasIndex(teacher => teacher.Code)
            .HasDatabaseName(IndexName(nameof(ArtemisTeacher.Code)));

        builder.HasIndex(teacher => teacher.Name)
            .HasDatabaseName(IndexName(nameof(ArtemisTeacher.Name)));
    }

    #endregion
}