using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
/// 教师变动记录配置
/// </summary>
internal sealed class ArtemisTeacherChangeLogConfiguration : ConcurrencyModelEntityConfiguration<ArtemisTeacherChangeLog>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisTeacher>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "教师变动记录数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisTeacherChangeLog).TableName();

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisTeacherChangeLog> builder)
    {
        builder.Property(teacher => teacher.ChangeTime)
            .HasColumnType(DataTypeSet.DateTime);
    }

    #endregion
}