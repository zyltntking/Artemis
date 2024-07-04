using Artemis.Data.Shared;
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
    protected override string TableName => nameof(ArtemisStudentCurrentAffiliation).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisStudentCurrentAffiliation> builder)
    {
        builder.HasKey(currentStudent => currentStudent.StudentId)
            .HasName(KeyName);

        // One Student One Current Students
        builder.HasOne(currentStudent => currentStudent.Student)
            .WithOne(student => student.CurrentAffiliation)
            .HasForeignKey<ArtemisStudentCurrentAffiliation>(currentStudent => currentStudent.StudentId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisStudentCurrentAffiliation).TableName(),
                nameof(ArtemisStudent).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // One School Many Current Students
        builder.HasOne(currentStudent => currentStudent.School)
            .WithMany(school => school.CurrentStudents)
            .HasForeignKey(currentStudent => currentStudent.SchoolId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisStudentCurrentAffiliation).TableName(),
                nameof(ArtemisSchool).TableName()))
            .OnDelete(DeleteBehavior.Cascade);

        // One Class Many Current Students
        builder.HasOne(currentStudent => currentStudent.Class)
            .WithMany(schoolClass => schoolClass.CurrentStudents)
            .HasForeignKey(currentStudent => currentStudent.ClassId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisStudentCurrentAffiliation).TableName(),
                nameof(ArtemisClass).TableName()))
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}