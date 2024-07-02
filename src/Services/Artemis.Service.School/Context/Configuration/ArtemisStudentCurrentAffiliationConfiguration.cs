using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     学生当前所属关系配置
/// </summary>
internal sealed class
    ArtemisStudentCurrentAffiliationConfiguration : BaseEntityConfiguration<ArtemisStudentCurrentAffiliation>
{
    #region Overrides of BaseConfiguration<ArtemisStudentCurrentAffiliation,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "学生当前所属关系数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisStudentCurrentAffiliation);

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisStudentCurrentAffiliation> builder)
    {
        builder.HasKey(currentStudent => currentStudent.StudentId)
            .HasName(KeyName);

        // Current Affiliation One School Many Student & One Class Many Students
        builder.HasOne(currentStudent => currentStudent.Student)
            .WithOne(student => student.CurrentAffiliation)
            .HasForeignKey<ArtemisStudentCurrentAffiliation>(currentStudent => currentStudent.StudentId)
            .HasConstraintName(nameof(ArtemisStudentCurrentAffiliation).ForeignKeyName(nameof(ArtemisStudent)))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(currentStudent => currentStudent.School)
            .WithMany(school => school.CurrentStudents)
            .HasForeignKey(currentStudent => currentStudent.SchoolId)
            .HasConstraintName(nameof(ArtemisStudentCurrentAffiliation).ForeignKeyName(nameof(ArtemisSchool)))
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(currentStudent => currentStudent.Class)
            .WithMany(schoolClass => schoolClass.CurrentStudents)
            .HasForeignKey(currentStudent => currentStudent.ClassId)
            .HasConstraintName(nameof(ArtemisStudentCurrentAffiliation).ForeignKeyName(nameof(ArtemisClass)))
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}