using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
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
    protected override string TableName => nameof(ArtemisTeacherCurrentAffiliation).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisTeacherCurrentAffiliation> builder)
    {
        builder.HasKey(currentAffiliation => new
            {
                currentAffiliation.SchoolId,
                currentAffiliation.ClassId,
                currentAffiliation.TeacherId
            })
            .HasName(KeyName);

        // One Teacher Many Current Affiliation
        builder.HasOne(currentAffiliation => currentAffiliation.Teacher)
            .WithMany(currentAffiliation => currentAffiliation.CurrentAffiliations)
            .HasForeignKey(currentAffiliation => currentAffiliation.TeacherId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisTeacherCurrentAffiliation).TableName(),
                nameof(ArtemisTeacher).TableName()))
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // One School Many Current Teachers
        builder.HasOne(currentAffiliation => currentAffiliation.School)
            .WithMany(currentAffiliation => currentAffiliation.CurrentTeachers)
            .HasForeignKey(currentAffiliation => currentAffiliation.SchoolId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisTeacherCurrentAffiliation).TableName(),
                nameof(ArtemisSchool).TableName()))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);

        // One Class Many Current Teachers
        builder.HasOne(currentAffiliation => currentAffiliation.Class)
            .WithMany(currentAffiliation => currentAffiliation.CurrentTeachers)
            .HasForeignKey(currentAffiliation => currentAffiliation.ClassId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisTeacherCurrentAffiliation).TableName(),
                nameof(ArtemisClass).TableName()))
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}