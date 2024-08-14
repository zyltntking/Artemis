using Artemis.Data.Core;
using Artemis.Service.Shared.Business.VisionScreen;

namespace Artemis.Service.Business.VisionScreen.Models;

/// <summary>
/// 机构用户绑定信息
/// </summary>
public class OrganizationUsersBinding : ConcurrencyPartition, IOrganizationUsersBinding
{
    #region Implementation of IOrganizationUsersBindingPackage

    /// <summary>
    /// 机构标识
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// 用户标识
    /// </summary>
    public Guid UserId { get; set; }

    #endregion
}