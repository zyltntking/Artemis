using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.App.BroadcastApi.Data.Configuration;

/// <summary>
///     用户配置
/// </summary>
public class UserConfiguration : BroadcastConfiguration<User>
{
    #region Overrides of BroadcastConfiguration<User>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "用户数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(User);

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<User> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(user => user.Id)
            .HasComment("标识");

        builder.Property(user => user.UserName)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("用户名");

        builder.Property(user => user.NormalizedUserName)
            .HasMaxLength(32)
            .IsRequired()
            .HasComment("规范化用户名");

        builder.Property(user => user.Password)
            .HasMaxLength(128)
            .IsRequired()
            .HasComment("凭据值");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<User> builder)
    {
        MetaIndexConfigure(builder);

        // User Key
        builder.HasKey(user => user.Id)
            .HasName($"PK_{TableName}");

        // User Name Index
        builder.HasIndex(user => user.NormalizedUserName)
            .HasDatabaseName($"IX_{TableName}_UserName");
    }

    #endregion
}