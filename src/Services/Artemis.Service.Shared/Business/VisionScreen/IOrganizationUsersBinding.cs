using Artemis.Data.Core;

namespace Artemis.Service.Shared.Business.VisionScreen;

/// <summary>
/// 机构用户绑定接口
/// </summary>
public interface IOrganizationUsersBinding : IOrganizationUsersBindingInfo;

/// <summary>
/// 机构用户绑定信息接口
/// </summary>
public interface IOrganizationUsersBindingInfo : IOrganizationUsersBindingPackage, IKeySlot;

/// <summary>
/// 机构用户绑定数据包接口
/// </summary>
public interface IOrganizationUsersBindingPackage
{
    /// <summary>
    /// 机构标识
    /// </summary>
    Guid OrganizationId { get; set; }

    /// <summary>
    /// 用户标识
    /// </summary>
    Guid UserId { get; set; }
}