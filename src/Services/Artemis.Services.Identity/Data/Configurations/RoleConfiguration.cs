﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     角色数据集配置
/// </summary>
public class RoleConfiguration : ArtemisIdentityConfiguration<ArtemisRole>
{
    #region Overrides of ArtemisConfiguration<ArtemisRole>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证角色数据集";

    /// <summary>
    ///     表配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void TableConfigure(EntityTypeBuilder<ArtemisRole> builder)
    {
        builder.ToTable(nameof(ArtemisRole), table => table.HasComment(DataSetDescription));
    }


    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisRole> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(role => role.Id)
            .HasComment("标识");

        builder.Property(role => role.Name)
            .HasMaxLength(128)
            .HasComment("角色名");

        builder.Property(role => role.NormalizedName)
            .HasMaxLength(128)
            .HasComment("规范化角色名");

        builder.Property(role => role.ConcurrencyStamp)
            .IsConcurrencyToken()
            .HasComment("并发戳");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisRole> builder)
    {
        // Role Key
        builder.HasKey(role => role.Id)
            .HasName($"PK_{nameof(ArtemisRole)}");

        // Role Index
        builder.HasIndex(role => role.NormalizedName)
            .HasDatabaseName($"IX_{nameof(ArtemisRole)}_Name")
            .IsUnique();

        // Each Role can have many entries in the UserRole join table
        builder.HasMany(role => role.UserRoles)
            .WithOne(userRole => userRole.Role)
            .HasForeignKey(userRole => userRole.RoleId)
            .HasConstraintName($"FK_{nameof(ArtemisUserRole)}_{nameof(ArtemisRole)}_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        // Each Role can have many associated RoleClaims
        builder.HasMany(role => role.RoleClaims)
            .WithOne(roleClaim => roleClaim.Role)
            .HasForeignKey(roleClaim => roleClaim.RoleId)
            .HasConstraintName($"FK_{nameof(ArtemisUserClaim)}_{nameof(ArtemisRole)}_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}