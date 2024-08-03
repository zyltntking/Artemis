using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Resource.Context;
using Artemis.Service.Resource.Stores;
using Artemis.Service.Shared.Resource.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Service.Resource.Managers;

/// <summary>
///     组织机构树管理器
/// </summary>
public interface IOrganizationTreeManager : ITreeManager<ArtemisOrganization, OrganizationInfo, OrganizationInfoTree,
    OrganizationPackage>
{
    /// <summary>
    ///     根据组织机构信息查搜索组织机构
    /// </summary>
    /// <param name="organizationNameSearch"></param>
    /// <param name="organizationCodeSearch"></param>
    /// <param name="organizationType"></param>
    /// <param name="organizationStatus"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<PageResult<OrganizationInfo>> FetchOrganizationsAsync(
        string? organizationNameSearch,
        string? organizationCodeSearch,
        string? organizationType,
        string? organizationStatus,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default);
}

/// <summary>
///     组织机构树管理器实现
/// </summary>
public class OrganizationTreeManager :
    TreeManager<ArtemisOrganization, OrganizationInfo, OrganizationInfoTree, OrganizationPackage>,
    IOrganizationTreeManager
{
    /// <summary>
    ///     树模型管理器构造
    /// </summary>
    public OrganizationTreeManager(IArtemisOrganizationStore organizationStore) : base(organizationStore)
    {
    }

    #region Implementation of IOrganizationTreeManager

    /// <summary>
    ///     根据组织机构信息查搜索组织机构
    /// </summary>
    /// <param name="organizationNameSearch"></param>
    /// <param name="organizationCodeSearch"></param>
    /// <param name="organizationType"></param>
    /// <param name="organizationStatus"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<PageResult<OrganizationInfo>> FetchOrganizationsAsync(
        string? organizationNameSearch,
        string? organizationCodeSearch,
        string? organizationType,
        string? organizationStatus,
        int page = 1,
        int size = 20,
        CancellationToken cancellationToken = default)
    {
        OnAsyncActionExecuting(cancellationToken);

        organizationNameSearch ??= string.Empty;
        organizationCodeSearch ??= string.Empty;
        organizationType ??= string.Empty;
        organizationStatus ??= string.Empty;

        var query = EntityStore.EntityQuery;

        var total = await query.LongCountAsync(cancellationToken);

        query = query.WhereIf(
            organizationNameSearch != string.Empty,
            organization => EF.Functions.Like(organization.Name, $"%{organizationNameSearch}%"));

        query = query.WhereIf(
            organizationCodeSearch != string.Empty,
            organization => EF.Functions.Like(organization.Code, $"%{organizationCodeSearch}%"));

        query = query.WhereIf(organizationType != string.Empty, organization => organization.Type == organizationType);

        query = query.WhereIf(organizationStatus != string.Empty,
            organization => organization.Status == organizationStatus);

        var count = await query.LongCountAsync(cancellationToken);

        query = query.OrderBy(division => division.CreateBy);

        if (page > 0 && size > 0) query = query.Page(page, size);

        var organizations = await query
            .ProjectToType<OrganizationInfo>()
            .ToListAsync(cancellationToken);

        return new PageResult<OrganizationInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = total,
            Items = organizations
        };
    }

    #endregion

    #region Overrides of TreeManager<ArtemisOrganization,Guid,Guid?,OrganizationInfo,OrganizationInfoTree,OrganizationPackage>

    /// <summary>
    /// 获取非根节点的树节点列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override Task<List<OrganizationInfo>> FetchNonRootTreeNodeList(Guid key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    #endregion
}