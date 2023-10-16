using Artemis.Data.Core;
using Artemis.Data.Store;
using Artemis.Data.Store.Extensions;
using Artemis.Extensions.Web.Filter;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Artemis.Extensions.Web.Controller;

/// <summary>
/// 抽象资源控制器
/// </summary>
/// <typeparam name="TResourceEntity">资源实体类型</typeparam>
/// <typeparam name="TResourceInfo">资源信息类型</typeparam>
/// <typeparam name="TResourcePack">资源包类型</typeparam>
[Route("api/[controller]")]
public abstract class ResourceController<TResourceEntity, TResourceInfo, TResourcePack> : ResourceController<TResourceEntity, Guid, TResourceInfo, TResourcePack> where TResourceEntity : class, IModelBase
{
    /// <summary>
    ///     泛型资源控制器
    /// </summary>
    /// <param name="manager">管理器依赖</param>
    /// <param name="logger">日志依赖</param>
    protected ResourceController(IManager<TResourceEntity> manager, ILogger logger) : base(manager, logger)
    {
    }
}

/// <summary>
/// 抽象资源控制器
/// </summary>
/// <typeparam name="TResourceEntity">资源实体类型</typeparam>
/// <typeparam name="TKey">资源键类型</typeparam>
/// <typeparam name="TResourceInfo">资源信息类型</typeparam>
/// <typeparam name="TResourcePack">资源包类型</typeparam>
[Route("api/[controller]")]
public abstract class ResourceController<TResourceEntity, TKey, TResourceInfo, TResourcePack> : ClaimedApiController 
    where TResourceEntity : class, IModelBase<TKey> 
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// 存储管理器依赖访问器
    /// </summary>
    private IManager<TResourceEntity, TKey> Manager { get; }

    /// <summary>
    ///     泛型资源控制器
    /// </summary>
    /// <param name="manager">管理器依赖</param>
    /// <param name="logger">日志依赖</param>
    protected ResourceController(
        IManager<TResourceEntity, TKey> manager,
        ILogger logger) : base(logger)
    {
        Manager = manager;
    }

    /// <summary>
    /// 获取资源列表
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <returns>Resources PagedResult</returns>
    /// <remark>GET api/Resources</remark>
    [HttpGet]
    public async Task<DataResult<PageResult<TResourceInfo>>> GetResources(int page = 1, int size = 20)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var query = Manager.EntityStore.EntityQuery;

        var count = await query.LongCountAsync(cancellationToken);

        var resources = await query
            .OrderByDescending(item => item.CreatedAt)
            .Page(page, size)
            .ProjectToType<TResourceInfo>()
            .ToListAsync(cancellationToken);

        var result = new PageResult<TResourceInfo>
        {
            Page = page,
            Size = size,
            Count = count,
            Total = count,
            Data = resources
        };

        return DataResult.Success(result);
    }

    /// <summary>
    /// 获取资源信息
    /// </summary>
    /// <param name="resourceId">资源标识</param>
    /// <returns>Resource Info Result</returns>
    /// <remark>GET api/Resources/{resourceId}</remark>
    [HttpGet("{resourceId}")]
    public async Task<DataResult<TResourceInfo>> GetResource(TKey resourceId)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var result = await Manager.EntityStore
            .FindMapEntityAsync<TResourceInfo>(resourceId, cancellationToken);

        return result is not null
            ? DataResult.Success(result)
            : DataResult.Fail<TResourceInfo>("未查询到匹配的资源");
    }

    /// <summary>
    /// 创建资源
    /// </summary>
    /// <param name="pack">资源信息包</param>
    /// <returns>Resource Info Result</returns>
    /// <remark>POST api/Resources</remark>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<DataResult<TResourceInfo>> PostResource([FromBody][Required] TResourcePack pack)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var resource = Instance.CreateInstance<TResourceEntity>();

        resource = pack.Adapt(resource);

        var result = await Manager.EntityStore.CreateAsync(resource, cancellationToken);

        return result.Succeeded
            ? DataResult.Success(resource.Adapt<TResourceInfo>())
            : DataResult.Fail<TResourceInfo>($"创建失败。{result.DescribeError}");
    }

    /// <summary>
    /// 更新资源
    /// </summary>
    /// <param name="resourceId">资源标识</param>
    /// <param name="pack">资源信息包</param>
    /// <returns>Resource Info Result</returns>
    /// <remark>PUT api/Resources/{resourceId}</remark>
    [HttpPut]
    public async Task<DataResult<TResourceInfo>> PutResource(TKey resourceId, [FromBody][Required] TResourcePack pack)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var exists = await Manager.EntityStore.ExistsAsync(resourceId, cancellationToken);

        if (!exists)
        {
            return DataResult.Fail<TResourceInfo>("资源不存在");
        }

        var resource = await Manager.EntityStore.FindEntityAsync(resourceId, cancellationToken);

        if (resource is null)
        {
            return DataResult.Fail<TResourceInfo>("资源不存在");
        }

        pack.Adapt(resource);

        var result = await Manager.EntityStore.UpdateAsync(resource, cancellationToken);

        return result.Succeeded
            ? DataResult.Success(resource.Adapt<TResourceInfo>())
            : DataResult.Fail<TResourceInfo>($"更新失败。{result.DescribeError}");
    }

    /// <summary>
    /// 删除资源
    /// </summary>
    /// <param name="resourceId">资源标识</param>
    /// <returns>Delete Status</returns>
    /// <remarks>DELETE api/Resources/{resourceId}</remarks>
    [HttpDelete]
    public async Task<DataResult<EmptyRecord>> DeleteResource(TKey resourceId)
    {
        var cancellationToken = HttpContext.RequestAborted;

        var exists = await Manager.EntityStore.ExistsAsync(resourceId, cancellationToken);

        if (!exists)
            return DataResult.Fail<EmptyRecord>("资源不存在");

        var resource = await Manager.EntityStore
            .KeyMatchQuery(resourceId)
            .FirstOrDefaultAsync(cancellationToken);

        if (resource == null)
            return DataResult.Fail<EmptyRecord>("资源不存在");

        var result = await Manager.EntityStore.DeleteAsync(resource, cancellationToken);

        return result.Succeeded
            ? DataResult.Success()
            : DataResult.Fail<EmptyRecord>($"删除失败。{result.DescribeError}");
    }
}

/// <summary>
///     泛型凭据认证Api控制器
/// </summary>
[ArtemisClaim]
[ApiController]
[Produces("application/json")]
[Route("api/[controller]/[action]")]
public abstract class ClaimedApiController : ApiController
{
    /// <summary>
    ///     泛型API控制器
    /// </summary>
    /// <param name="logger"></param>
    protected ClaimedApiController(ILogger logger) : base(logger)
    {
    }
}

/// <summary>
///     泛型Api控制器
/// </summary>
[ApiController]
[Produces("application/json")]
[Route("api/[controller]/[action]")]
public abstract class ApiController : ControllerBase
{
    /// <summary>
    ///     泛型API控制器
    /// </summary>
    /// <param name="logger"></param>
    protected ApiController(ILogger logger)
    {
        Logger = logger;
    }

    /// <summary>
    ///     日志访问器
    /// </summary>
    protected ILogger Logger { get; }
}