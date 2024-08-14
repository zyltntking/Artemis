using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Business.VisionScreen.Context.Configuration;

/// <summary>
///     用户学生亲属关系绑定数据集配置
/// </summary>
internal sealed class ArtemisStudentRelationBindingConfiguration : 
    ConcurrencyPartitionEntityConfiguration<ArtemisStudentRelationBinding>
{
    #region Overrides

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "用户学生亲属关系绑定数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisStudentRelationBinding).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisStudentRelationBinding> builder)
    {
        // index
        builder.HasIndex(bind => bind.UserId)
            .HasDatabaseName(IndexName(nameof(ArtemisStudentRelationBinding.UserId)));

        builder.HasIndex(bind => bind.StudentId)
            .HasDatabaseName(nameof(ArtemisStudentRelationBinding.StudentId));
    }


    #endregion
}