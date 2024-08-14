using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Business.VisionScreen.Context.Configuration;

/// <summary>
///     用户教师绑定据集配置
/// </summary>
internal sealed class ArtemisTeacherUserBindingConfiguration :
    ConcurrencyPartitionEntityConfiguration<ArtemisTeacherUserBinding>
{
    #region Overrides

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "用户教师绑定数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisTeacherUserBinding).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisTeacherUserBinding> builder)
    {
        // index
        builder.HasIndex(bind => bind.TeacherId)
            .HasDatabaseName(IndexName(nameof(ArtemisTeacherUserBinding.TeacherId)))
            .IsUnique();

        builder.HasIndex(bind => bind.UserId)
            .HasDatabaseName(nameof(ArtemisTeacherUserBinding.UserId))
            .IsUnique();
    }

    #endregion
}