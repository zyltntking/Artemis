using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证用户角色登录实体配置
/// </summary>
internal class IdentityUserLoginConfiguration : BaseEntityConfiguration<IdentityUserLogin>
{
    #region Overrides of BaseEntityConfiguration<IdentityUserLogin,Guid,Guid,string,int>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户角色登录数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityUserLogin);

    /// <summary>
    ///     实体关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void EntityRelationConfigure(EntityTypeBuilder<IdentityUserLogin> builder)
    {
        // User Login Key
        builder.HasKey(userLogin => new { userLogin.LoginProvider, userLogin.ProviderKey })
            .HasName(KeyName);
    }

    #endregion
}