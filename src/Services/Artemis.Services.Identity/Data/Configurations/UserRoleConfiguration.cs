using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
///     用户角色映射数据集配置
/// </summary>
public class UserRoleConfiguration : ArtemisIdentityConfiguration<ArtemisUserRole>
{
    #region Overrides of ArtemisConfiguration<ArtemisUserRole>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户角色映射数据集";

    /// <summary>
    ///     表配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void TableConfigure(EntityTypeBuilder<ArtemisUserRole> builder)
    {
        builder.ToTable(nameof(ArtemisUserRole), table => table.HasComment(DataSetDescription));
    }

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisUserRole> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(userRole => userRole.UserId)
            .HasComment("用户标识");

        builder.Property(userRole => userRole.RoleId)
            .HasComment("角色标识");
    }

    /// <summary>
    ///     数据库关系配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void RelationConfigure(EntityTypeBuilder<ArtemisUserRole> builder)
    {
        // User Role Key
        builder.HasKey(userRole => new { userRole.UserId, userRole.RoleId })
            .HasName($"PK_{nameof(ArtemisUserRole)}");
    }

    #endregion
}