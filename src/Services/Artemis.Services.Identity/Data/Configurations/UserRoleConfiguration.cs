using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Artemis.Services.Identity.Data.Configurations;

/// <summary>
/// 用户角色映射数据集配置
/// </summary>
public class UserRoleConfiguration : ArtemisIdentityConfiguration<ArtemisIdentityUserRole>
{
    #region Overrides of ArtemisConfiguration<ArtemisIdentityUserRole>

    /// <summary>
    ///   数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户角色映射数据集";

    /// <summary>
    ///     数据库字段配置
    /// </summary>
    /// <param name="builder"></param>
    protected override void FieldConfigure(EntityTypeBuilder<ArtemisIdentityUserRole> builder)
    {
        base.FieldConfigure(builder);

        builder.Property(entity => entity.UserId)
            .HasComment("用户标识");

        builder.Property(entity => entity.RoleId)
            .HasComment("角色标识");
    }

    #endregion
}