using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     班级教师关系对应实体配置
/// </summary>
internal sealed class ArtemisClassTeacherConfiguration : BaseEntityConfiguration<ArtemisClassTeacher>
{
    #region Overrides of BaseConfiguration<IdentityUserRole>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "班级教师映射数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => "ClassTeacher";

    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisClassTeacher> builder)
    {
        builder.Property(classStudent => classStudent.MoveIn)
            .HasComment("转入时间")
            .HasColumnType(DataTypeSet.DateTime);

        builder.Property(classStudent => classStudent.MoveOut)
            .HasComment("转出时间")
            .HasColumnType(DataTypeSet.DateTime);
    }

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisClassTeacher> builder)
    {
        builder.HasIndex(classStudent => classStudent.MoveIn)
            .HasDatabaseName(IndexName("MoveIn"));

        builder.HasIndex(classStudent => classStudent.MoveOut)
            .HasDatabaseName(IndexName("MoveOut"));
    }

    #endregion
}