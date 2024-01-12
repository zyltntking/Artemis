using Artemis.Data.Store.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     用户登录数据集
/// </summary>
public class UserLoginConfiguration : BaseConfiguration<ArtemisUserLogin>
{
    #region Overrides of ModelBaseConfiguration<ArtemisUserLogin>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户登录数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(ArtemisUserLogin);


    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisUserLogin> builder)
    {
        builder.Property(userLogin => userLogin.UserId)
            .HasComment("用户标识");

        builder.Property(userLogin => userLogin.LoginProvider)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("认证提供程序");

        builder.Property(userLogin => userLogin.ProviderKey)
            .HasMaxLength(64)
            .IsRequired()
            .HasComment("认证提供程序提供的第三方标识");

        builder.Property(userLogin => userLogin.ProviderDisplayName)
            .HasMaxLength(32)
            .HasComment("认证提供程序显示的用户名");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisUserLogin> builder)
    {
        // User Login Key
        builder.HasKey(userLogin => new { userLogin.LoginProvider, userLogin.ProviderKey })
            .HasName($"PK_{TableName}");
    }

    #endregion
}