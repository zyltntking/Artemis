using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     教师当前所属关系配置
/// </summary>
internal sealed class
    ArtemisTeacherCurrentAffiliationConfiguration : BaseEntityConfiguration<ArtemisTeacherCurrentAffiliation>
{
    #region Overrides of BaseEntityConfiguration<ArtemisTeacherCurrentAffiliation,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "教师当前所属关系数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisTeacherCurrentAffiliation);

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisTeacherCurrentAffiliation> builder)
    {
        builder.HasKey(currentTeacher => currentTeacher.TeacherId)
            .HasName(KeyName);

        // Current Affiliation One School Many Teachers
        builder.HasOne(currentTeacher => currentTeacher.Teacher)
            .WithOne(teacher => teacher.CurrentSchool)
            .HasForeignKey<ArtemisTeacherCurrentAffiliation>(currentTeacher => currentTeacher.TeacherId)
            .HasConstraintName(nameof(ArtemisTeacherCurrentAffiliation).ForeignKeyName(nameof(ArtemisTeacher)))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(currentTeacher => currentTeacher.School)
            .WithMany(school => school.CurrentTeachers)
            .HasForeignKey(currentTeacher => currentTeacher.SchoolId)
            .HasConstraintName(nameof(ArtemisTeacherCurrentAffiliation).ForeignKeyName(nameof(ArtemisSchool)))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}