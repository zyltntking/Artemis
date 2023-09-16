using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     用户令牌数据集配置
/// </summary>
public class UserTokenConfiguration : ArtemisIdentityConfiguration<ArtemisUserToken>
{
    #region Overrides of ArtemisConfiguration<ArtemisUserToken>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户令牌数据集";

    /// <summary>
    ///     表配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void TableConfigure(EntityTypeBuilder<ArtemisUserToken> builder)
    {
        builder.ToTable(nameof(ArtemisUserToken), table => table.HasComment(DataSetDescription));
    }

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisUserToken> builder)
    {
        builder.Property(user => user.Id)
            .ValueGeneratedOnAdd()
            .HasComment("标识");

        builder.Property(userToken => userToken.UserId)
            .HasComment("用户标识");

        builder.Property(userToken => userToken.LoginProvider)
            .HasMaxLength(256)
            .HasComment("认证提供程序");

        builder.Property(userToken => userToken.Name)
            .HasMaxLength(256)
            .HasComment("认证令牌名");

        builder.Property(userToken => userToken.Value)
            .HasComment("认证令牌");

        base.FieldConfigure(builder);
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisUserToken> builder)
    {
        // User Token Key
        builder.HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name })
            .HasName($"PK_{nameof(ArtemisUserToken)}");

        // User Token Alternate Key
        builder.HasAlternateKey(userToken => userToken.Id)
            .HasName($"AK_{nameof(ArtemisUserToken)}");
    }

    #endregion
}