﻿using Microsoft.EntityFrameworkCore;
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
        base.FieldConfigure(builder);

        builder.Property(user => user.Id)
            .HasComment("标识");

        builder.Property(userLogin => userLogin.UserId)
            .HasComment("用户标识");

        builder.Property(userLogin => userLogin.LoginProvider)
            .HasMaxLength(256)
            .HasComment("认证提供程序");

        builder.Property(userLogin => userLogin.ProviderKey)
            .HasMaxLength(256)
            .HasComment("认证提供程序所需的Key");

        builder.Property(userLogin => userLogin.ProviderDisplayName)
            .HasMaxLength(128)
            .HasComment("认证提供程序显示的用户名");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisUserLogin> builder)
    {
        // User Login Key
        builder.HasKey(userLogin => userLogin.Id)
            .HasName($"PK_{nameof(ArtemisUserLogin)}");

        // User Login Index
        builder.HasIndex(userLogin => new { userLogin.LoginProvider, userLogin.ProviderKey })
            .HasDatabaseName($"IX_{nameof(ArtemisUserLogin)}_LoginProvider_ProviderKey")
            .IsUnique();

        //builder.HasKey(userLogin => new { userLogin.LoginProvider, userLogin.ProviderKey })
        //    .HasName($"PK_{nameof(ArtemisUserLogin)}");
    }

    #endregion
}