using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证用户角色登录实体配置
/// </summary>
internal class IdentityUserProfileConfiguration : BaseEntityConfiguration<IdentityUserProfile>
{
    #region Overrides of BaseEntityConfiguration<IdentityUserProfile,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户角色档案数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityUserProfile).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<IdentityUserProfile> builder)
    {
        // User profile Key
        builder.HasKey(userProfile => new { userProfile.UserId, userProfile.Key })
            .HasName(KeyName);
    }

    #endregion
}