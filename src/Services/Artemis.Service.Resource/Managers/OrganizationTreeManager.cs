using System.Linq.Dynamic.Core;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Design;
using Artemis.Data.Core.Fundamental.Types;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Service.Resource.Context;
using Artemis.Service.Resource.Stores;
using Artemis.Service.Shared.Resource.Transfer;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

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
    public OrganizationTreeManager(
        IArtemisOrganizationStore organizationStore,
        IOptions<ResourceServiceConfig> options) : base(organizationStore)
    {
        Options = options.Value;
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

    /// <summary>
    ///     资源配置
    /// </summary>
    private ResourceServiceConfig Options { get; }

    #region Overrides

    private string? _organizationDesignCodes;

    private int? _organizationSerial;

    /// <summary>
    ///     映射到新实体
    /// </summary>
    /// <param name="package"></param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<ArtemisOrganization> MapNewEntity(
        OrganizationPackage package,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        var entity = await base.MapNewEntity(package, loopIndex, cancellationToken);

        _organizationDesignCodes ??= await EntityStore
            .EntityQuery
            .Where(organization => organization.ParentId == null)
            .Select(organization => organization.DesignCode)
            .MaxAsync(cancellationToken);

        _organizationSerial ??= _organizationDesignCodes[^3..].Adapt<int>();

        string region;

        int serial;

        if (string.IsNullOrWhiteSpace(_organizationDesignCodes))
        {
            region = Options.OrganizationRegion;
            serial = loopIndex;
        }
        else
        {
            region = _organizationDesignCodes[3..7];
            serial = (int)(_organizationSerial + loopIndex);
        }

        entity.Status = OrganizationStatus.InOperation;
        entity.DesignCode = DesignCode.Organization(region, serial);

        return entity;
    }

    private string? _childOrganizationDesignCodes;

    private int? _childOrganizationSerial;

    /// <summary>
    ///     在添加子节点之前
    /// </summary>
    /// <param name="parent">父节点</param>
    /// <param name="child">子节点</param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    protected override async Task BeforeAddChildNode(
        ArtemisOrganization parent,
        ArtemisOrganization child,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        _childOrganizationDesignCodes ??= await EntityStore
            .EntityQuery
            .Where(organization => organization.ParentId == parent.Id)
            .Select(organization => organization.DesignCode)
            .MaxAsync(cancellationToken);

        _childOrganizationSerial ??= _childOrganizationDesignCodes[^3..].Adapt<int>();

        var region = parent.DesignCode[3..7];

        var serial = (int)(_childOrganizationSerial + loopIndex);

        child.DesignCode = DesignCode.Organization(region, serial, parent.DesignCode);
    }

    /// <summary>
    ///     在移除子节点之前
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="child">子节点</param>
    /// <param name="loopIndex"></param>
    /// <param name="cancellationToken"></param>
    protected override async Task BeforeRemoveChildNode(
        ArtemisOrganization parent,
        ArtemisOrganization child,
        int loopIndex,
        CancellationToken cancellationToken = default)
    {
        _organizationDesignCodes ??= await EntityStore
            .EntityQuery
            .Where(organization => organization.ParentId == null)
            .Select(organization => organization.DesignCode)
            .MaxAsync(cancellationToken);

        _organizationSerial ??= _organizationDesignCodes[^3..].Adapt<int>();

        string region;

        int serial;

        if (string.IsNullOrWhiteSpace(_organizationDesignCodes))
        {
            region = Options.OrganizationRegion;
            serial = loopIndex;
        }
        else
        {
            region = _organizationDesignCodes[3..7];
            serial = (int)(_organizationSerial + loopIndex);
        }

        child.DesignCode = DesignCode.Organization(region, serial);
    }

    /// <summary>
    ///     获取非根节点的树节点列表
    /// </summary>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<List<OrganizationInfo>> FetchNonRootTreeNodeList(Guid key,
        CancellationToken cancellationToken)
    {
        var designCode = await EntityStore
            .KeyMatchQuery(key)
            .Select(organization => organization.DesignCode)
            .FirstOrDefaultAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(designCode))
        {
            var list = await EntityStore.EntityQuery
                .Where(organization => organization.DesignCode.StartsWith(designCode))
                .ProjectToType<OrganizationInfo>()
                .ToListAsync(cancellationToken);

            return list;
        }

        return [];
    }

    #endregion
}