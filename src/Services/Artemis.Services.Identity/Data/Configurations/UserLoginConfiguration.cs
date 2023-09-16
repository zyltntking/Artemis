using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     用户登录数据集
/// </summary>
public class UserLoginConfiguration : ArtemisIdentityConfiguration<ArtemisUserLogin>
{
    #region Overrides of ArtemisConfiguration<ArtemisUserLogin>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户登录数据集";

    /// <summary>
    ///     表配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void TableConfigure(EntityTypeBuilder<ArtemisUserLogin> builder)
    {
        builder.ToTable(nameof(ArtemisUserLogin), table => table.HasComment(DataSetDescription));
    }

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisUserLogin> builder)
    {
        builder.Property(user => user.Id)
            .ValueGeneratedOnAdd()
            .HasComment("标识");

        builder.Property(userLogin => userLogin.UserId)
            .HasComment("用户标识");

        builder.Property(userLogin => userLogin.LoginProvider)
            .HasComment("认证提供程序");

        builder.Property(userLogin => userLogin.ProviderKey)
            .HasComment("认证提供程序提供的第三方标识");

        builder.Property(userLogin => userLogin.ProviderDisplayName)
            .HasComment("认证提供程序显示的用户名");

        base.FieldConfigure(builder);
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisUserLogin> builder)
    {
        // User Login Key
        builder.HasKey(userLogin => new { userLogin.LoginProvider, userLogin.ProviderKey })
            .HasName($"PK_{nameof(ArtemisUserLogin)}");

        // User Login Alternate Key
        builder.HasAlternateKey(userLogin => userLogin.Id)
            .HasName($"AK_{nameof(ArtemisUserLogin)}");
    }

    #endregion
}