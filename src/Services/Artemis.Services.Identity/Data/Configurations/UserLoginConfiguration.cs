using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
/// 用户登录数据集
/// </summary>
public sealed class UserLoginConfiguration : ArtemisIdentityConfiguration<ArtemisIdentityUserLogin>
{
    #region Overrides of ArtemisConfiguration<ArtemisIdentityUserLogin>

    /// <summary>
    ///   数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户登录数据集";

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisIdentityUserLogin> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(entity => entity.UserId)
            .HasComment("用户标识");

        builder.Property(entity => entity.LoginProvider)
            .HasComment("认证提供程序");

        builder.Property(entity => entity.ProviderKey)
            .HasComment("认证提供程序所需的Key");

        builder.Property(entity => entity.ProviderDisplayName)
            .HasComment("认证提供程序显示的用户名");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisIdentityUserLogin> builder)
    {
        builder.HasIndex(entity => entity.LoginProvider);
    }

    #endregion
}