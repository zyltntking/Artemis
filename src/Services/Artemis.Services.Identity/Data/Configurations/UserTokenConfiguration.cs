using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
/// 用户令牌数据集配置
/// </summary>
public class UserTokenConfiguration : ArtemisIdentityConfiguration<ArtemisIdentityUserToken>
{
    #region Overrides of ArtemisConfiguration<ArtemisIdentityUserToken>

    /// <summary>
    ///   数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户令牌数据集";

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisIdentityUserToken> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(userToken => userToken.UserId)
            .HasComment("用户标识");

        builder.Property(userToken => userToken.LoginProvider)
            .HasMaxLength(256).HasComment("认证提供程序");

        builder.Property(userToken => userToken.Name)
            .HasMaxLength(256).HasComment("认证令牌名");

        builder.Property(userToken => userToken.Value)
            .HasComment("认证令牌");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisIdentityUserToken> builder)
    {
        // User Token Key
        builder.HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name }).HasName("PK_UserTokens");
    }

    #endregion
}