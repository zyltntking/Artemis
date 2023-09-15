﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     用户凭据数据集配置
/// </summary>
public class UserClaimConfiguration : ArtemisIdentityConfiguration<ArtemisUserClaim>
{
    #region Overrides of ArtemisConfiguration<ArtemisUserClaim>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户数据集";

    /// <summary>
    ///     表配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void TableConfigure(EntityTypeBuilder<ArtemisUserClaim> builder)
    {
        builder.ToTable(nameof(ArtemisUserClaim), table => table.HasComment(DataSetDescription));
    }

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisUserClaim> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(userClaim => userClaim.Id)
            .HasComment("标识");

        builder.Property(userClaim => userClaim.UserId)
            .HasComment("用户标识");

        builder.Property(userClaim => userClaim.ClaimType)
            .HasMaxLength(128)
            .HasComment("凭据类型");

        builder.Property(userClaim => userClaim.ClaimValue)
            .HasMaxLength(128)
            .HasComment("凭据类型");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisUserClaim> builder)
    {
        // User Claim Key
        builder.HasKey(userClaim => userClaim.Id)
            .HasName($"PK_{nameof(ArtemisUserClaim)}");

        // User Claim Index
        builder.HasIndex(userClaim => userClaim.ClaimType)
            .HasDatabaseName($"IX_{nameof(ArtemisUserClaim)}_ClaimType");
    }

    #endregion
}