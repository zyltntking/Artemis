using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
/// 用户令牌数据集配置
/// </summary>
public sealed class UserTokenConfiguration : ArtemisIdentityConfiguration<ArtemisIdentityUserToken>
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

        builder.Property(entity => entity.UserId)
            .HasComment("用户标识");

        builder.Property(entity => entity.LoginProvider)
            .HasComment("认证提供程序");

        builder.Property(entity => entity.Name)
            .HasComment("认证令牌名");

        builder.Property(entity => entity.Value)
            .HasComment("认证令牌");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisIdentityUserToken> builder)
    {
        builder.HasIndex(entity => entity.LoginProvider);
    }

    #endregion
}