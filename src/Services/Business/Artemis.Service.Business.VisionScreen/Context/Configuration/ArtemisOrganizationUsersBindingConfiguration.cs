using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Business.VisionScreen.Context.Configuration;

/// <summary>
///     机构用户关系绑定数据集配置
/// </summary>
internal sealed class ArtemisOrganizationUsersBindingConfiguration : 
    ConcurrencyPartitionEntityConfiguration<ArtemisOrganizationUsersBinding>
{
    #region Overrides

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "机构用户关系绑定数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisOrganizationUsersBinding).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<ArtemisOrganizationUsersBinding> builder)
    {
        // index
        builder.HasIndex(bind => bind.UserId)
            .HasDatabaseName(IndexName(nameof(ArtemisOrganizationUsersBinding.UserId)));

        builder.HasIndex(bind => bind.OrganizationId)
            .HasDatabaseName(nameof(ArtemisOrganizationUsersBinding.OrganizationId));
    }

    #endregion
}