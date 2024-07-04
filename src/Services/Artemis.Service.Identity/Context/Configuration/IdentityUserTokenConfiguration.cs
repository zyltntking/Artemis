using Artemis.Data.Shared;
using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证用户令牌实体配置
/// </summary>
public class IdentityUserTokenConfiguration : BaseEntityConfiguration<IdentityUserToken>
{
    #region Overrides of BaseEntityConfiguration<IdentityUserToken,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户令牌数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityUserToken).TableName();

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<IdentityUserToken> builder)
    {
        // User Token Key
        builder.HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name })
            .HasName(KeyName);
    }

    #endregion
}