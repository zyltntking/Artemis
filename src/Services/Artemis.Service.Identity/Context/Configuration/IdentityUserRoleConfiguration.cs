using Artemis.Data.Store.Configuration;
using Artemis.Service.Shared;

namespace Artemis.Service.Identity.Context.Configuration;

/// <summary>
///     认证用户角色映射实体配置
/// </summary>
internal sealed class IdentityUserRoleConfiguration : BaseEntityConfiguration<IdentityUserRole>
{
    #region Overrides of BaseConfiguration<IdentityUserRole>

    /// <summary>
    ///     数据集描述
    /// </summary>
    protected override string DataSetDescription => "认证用户角色映射数据集";

    /// <summary>
    ///     表名
    /// </summary>
    protected override string TableName => nameof(IdentityUserRole).TableName();

    #endregion
}