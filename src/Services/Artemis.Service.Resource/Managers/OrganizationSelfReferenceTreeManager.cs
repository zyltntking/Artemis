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
public interface IOrganizationTreeManager :
    ISelfReferenceTreeManager<
        ArtemisOrganization,
        OrganizationInfo,
        OrganizationInfoTree,
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
public class OrganizationSelfReferenceTreeManager :
    SelfReferenceTreeManager<
        ArtemisOrganization,
        OrganizationInfo,
        OrganizationInfoTree,
        OrganizationPackage>,
    IOrganizationTreeManager
{
    /// <summary>
    ///     树模型管理器构造
    /// </summary>
    public OrganizationSelfReferenceTreeManager(
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

    /// <summary>
    ///     区域编码
    /// </summary>
    private string? _region;

    /// <summary>
    ///     机构序列
    /// </summary>
    private int? _serial;

    /// <summary>
    ///     子机构序列
    /// </summary>
    private int? _childSerial;

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

        if (string.IsNullOrEmpty(_region) || _serial is null)
        {
            var organizationDesignCodes = await EntityStore
                .EntityQuery
                .Where(organization => organization.ParentId == null)
                .Select(organization => organization.DesignCode)
                .MaxAsync(cancellationToken);

            if (string.IsNullOrEmpty(organizationDesignCodes))
            {
                _region = Options.OrganizationRegion;
                _serial = 0;
            }
            else
            {
                _region = organizationDesignCodes[3..7];
                _serial = organizationDesignCodes[^3..].Adapt<int>();
            }
        }

        entity.Status = OrganizationStatus.InOperation;
        entity.DesignCode = DesignCode.Organization(_region, (int)(_serial + loopIndex));

        entity.Code = entity.DesignCode;

        return entity;
    }

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
        if (string.IsNullOrWhiteSpace(_region) || _childSerial is null)
        {
            var childOrganizationDesignCodes = await EntityStore
                .EntityQuery
                .Where(organization => organization.ParentId == parent.Id)
                .Select(organization => organization.DesignCode)
                .MaxAsync(cancellationToken);

            if (string.IsNullOrEmpty(childOrganizationDesignCodes))
            {
                _region = Options.OrganizationRegion;
                _childSerial = 0;
            }
            else
            {
                _region = childOrganizationDesignCodes[3..7];
                _childSerial = childOrganizationDesignCodes[^3..].Adapt<int>();
            }
        }

        child.DesignCode = DesignCode.Organization(_region, (int)(_childSerial + loopIndex), parent.DesignCode);

        child.Code = child.DesignCode;

        await FlushChildrenDesignCode(child.Id, child.DesignCode, cancellationToken);
    }


    /// <summary>
    ///     刷新子节点的设计编码
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="parentDesignCode"></param>
    /// <param name="cancellationToken"></param>
    private async Task FlushChildrenDesignCode(
        Guid parentId,
        string parentDesignCode,
        CancellationToken cancellationToken = default)
    {
        var children = await EntityStore.EntityQuery
            .Where(organization => organization.ParentId == parentId)
            .ToListAsync(cancellationToken);

        if (children.Any())
        {
            var index = 1;

            foreach (var child in children)
            {
                if (string.IsNullOrWhiteSpace(_region))
                    _region = string.IsNullOrWhiteSpace(child.DesignCode)
                        ? Options.OrganizationRegion
                        : child.DesignCode[3..7];

                child.DesignCode = DesignCode.Organization(_region, index, parentDesignCode);

                child.Code = child.DesignCode;

                index++;

                await FlushChildrenDesignCode(child.Id, child.DesignCode, cancellationToken);
            }

            await EntityStore.UpdateAsync(children, cancellationToken);
        }
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
        if (string.IsNullOrWhiteSpace(_region) || _serial is null)
        {
            var organizationDesignCodes = await EntityStore
                .EntityQuery
                .Where(organization => organization.ParentId == null)
                .Select(organization => organization.DesignCode)
                .MaxAsync(cancellationToken);

            if (string.IsNullOrEmpty(organizationDesignCodes))
            {
                _region = Options.OrganizationRegion;
                _serial = 0;
            }
            else
            {
                _region = organizationDesignCodes[3..7];
                _serial = organizationDesignCodes[^3..].Adapt<int>();
            }
        }

        child.DesignCode = DesignCode.Organization(_region, (int)(_serial + loopIndex));

        child.Code = child.DesignCode;
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