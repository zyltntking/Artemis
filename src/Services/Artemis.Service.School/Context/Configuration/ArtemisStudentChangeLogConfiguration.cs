using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
/// 学生变动记录配置
/// </summary>
internal sealed class ArtemisStudentChangeLogConfiguration : ConcurrencyModelEntityConfiguration<ArtemisStudentChangeLog>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisTeacher>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "学生变动记录数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisStudentChangeLog).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisStudentChangeLog> builder)
    {
        builder.Property(student => student.ChangeTime)
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(student => student.Birthday)
            .HasColumnType(DataTypeSet.DateTime);
    }

    #endregion
}