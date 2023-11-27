using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     用户档案数据集
/// </summary>
public class UserProfileConfiguration : IdentityConfiguration<ArtemisUserProfile>
{
    #region Overrides of ArtemisConfiguration<ArtemisUserProfile>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户信息数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisUserProfile);


    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisUserProfile> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(userProfile => userProfile.Id)
            .HasComment("标识");

        builder.Property(userProfile => userProfile.UserId)
            .HasComment("用户标识");

        builder.Property(userProfile => userProfile.Key)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("用户信息键");

        builder.Property(userProfile => userProfile.Value)
            .HasMaxLength(128)
            .HasComment("用户信息值");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisUserProfile> builder)
    {
        MetaIndexConfigure(builder);

        // User Profile Key
        builder.HasKey(userProfile => userProfile.Id)
            .HasName($"PK_{TableName}");

        // User Profile Index
        builder.HasIndex(userLogin => new { userLogin.Key, userLogin.UserId })
            .HasDatabaseName($"IX_{TableName}_Key_UserId")
            .IsUnique();

        builder.HasIndex(userLogin => new { userLogin.Key, userLogin.Value })
            .HasDatabaseName($"IX_{TableName}_Key_Value")
            .IsUnique();
    }

    #endregion
}