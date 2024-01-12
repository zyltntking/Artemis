using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     用户令牌数据集配置
/// </summary>
public class UserTokenConfiguration : BaseConfiguration<ArtemisUserToken>
{
    #region Overrides of ModelBaseConfiguration<ArtemisUserToken>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户令牌数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisUserToken);

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisUserToken> builder)
    {
        builder.Property(userToken => userToken.UserId)
            .HasComment("用户标识");

        builder.Property(userToken => userToken.LoginProvider)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("认证提供程序");

        builder.Property(userToken => userToken.Name)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("认证令牌名");

        builder.Property(userToken => userToken.Value)
            .HasComment("认证令牌");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisUserToken> builder)
    {
        // User Token Key
        builder.HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name })
            .HasName($"PK_{TableName}");
    }

    #endregion
}