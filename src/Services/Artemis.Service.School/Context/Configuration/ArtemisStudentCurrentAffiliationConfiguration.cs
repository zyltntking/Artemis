using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
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
        builder.HasKey(currentAffiliation => currentAffiliation.StudentId)
            .HasName(KeyName);

        // One Student One Current Affiliation Student
        builder.HasOne(currentAffiliation => currentAffiliation.Student)
            .WithOne(student => student.CurrentAffiliation)
            .HasForeignKey<ArtemisStudentCurrentAffiliation>(currentAffiliation => currentAffiliation.StudentId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisStudentCurrentAffiliation).TableName(),
                nameof(ArtemisStudent).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // One School Many Current Affiliation Student
        builder.HasOne(currentAffiliation => currentAffiliation.School)
            .WithMany(school => school.CurrentStudents)
            .HasForeignKey(currentAffiliation => currentAffiliation.SchoolId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisStudentCurrentAffiliation).TableName(),
                nameof(ArtemisSchool).TableName()))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        // One Class Many Current Affiliation Student
        builder.HasOne(currentAffiliation => currentAffiliation.Class)
            .WithMany(schoolClass => schoolClass.CurrentStudents)
            .HasForeignKey(currentAffiliation => currentAffiliation.ClassId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisStudentCurrentAffiliation).TableName(),
                nameof(ArtemisClass).TableName()))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}