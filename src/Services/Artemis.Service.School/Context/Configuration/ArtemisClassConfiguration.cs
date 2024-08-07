using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.School.Context.Configuration;

/// <summary>
///     班级配置
/// </summary>
internal sealed class ArtemisClassConfiguration : ConcurrencyModelEntityConfiguration<ArtemisClass>
{
    #region Overrides of ConcurrencyPartitionEntityConfiguration<ArtemisClass>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "班级数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisClass).TableName();


    /// <summary>
    ///     实体字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityFieldConfigure(EntityTypeBuilder<ArtemisClass> builder)
    {
        builder.Property(entity => entity.EstablishTime)
            .HasColumnType(DataTypeSet.DateTime);
    }


    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisClass> builder)
    {
        // index
        builder.HasIndex(iClass => iClass.Name)
            .HasDatabaseName(IndexName(nameof(ArtemisClass.Name)));

        builder.HasIndex(iClass => iClass.GradeName)
            .HasDatabaseName(IndexName(nameof(ArtemisClass.GradeName)));

        builder.HasIndex(iClass => iClass.Code)
            .HasDatabaseName(IndexName(nameof(ArtemisClass.Code)));

        builder.HasIndex(iClass => iClass.StudyPhase)
            .HasDatabaseName(IndexName(nameof(ArtemisClass.StudyPhase)));

        builder.HasIndex(iClass => iClass.Length)
            .HasDatabaseName(IndexName(nameof(ArtemisClass.Length)));

        builder.HasIndex(iClass => iClass.SerialNumber)
            .HasDatabaseName(IndexName(nameof(ArtemisClass.SerialNumber)));

        // Each Class can have one HeadTeacher
        builder.HasOne(iClass => iClass.HeadTeacher)
            .WithOne(teacher => teacher.HeadTeacherClass)
            .HasForeignKey<ArtemisClass>(iClass => iClass.HeadTeacherId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisTeacher).TableName(),
                nameof(ArtemisClass).TableName()))
            .IsRequired(false);

        // Each Class can have many Students
        builder.HasMany(iClass => iClass.Students)
            .WithOne(student => student.Class)
            .HasForeignKey(student => student.ClassId)
            .HasConstraintName(ForeignKeyName(
                nameof(ArtemisClass).TableName(),
                nameof(ArtemisStudent).TableName()))
            .IsRequired(false);
    }

    #endregion
}