using System.Linq.Expressions;
using Artemis.Data.Core;

namespace Artemis.Data.Store.Extensions;

/// <summary>
///     Queryable扩展
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    ///     分页扩展
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="page">页码</param>
    /// <param name="size">数据条目</param>
    /// <returns></returns>
    public static IQueryable<TEntity> Page<TEntity>(
        this IQueryable<TEntity> query,
        int page,
        int size)
    {
        return query.Skip((page - 1) * size).Take(size);
    }

    /// <summary>
    ///     分页扩展
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="page">页码</param>
    /// <param name="size">数据条目</param>
    /// <param name="count">数据规模</param>
    /// <returns></returns>
    public static IQueryable<TEntity> Page<TEntity>(
        this IQueryable<TEntity> query,
        int page,
        int size,
        out long count)
    {
        count = query.LongCount();
        return query.Skip((page - 1) * size).Take(size);
    }

    /// <summary>
    ///     分页扩展
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="request">分页请求</param>
    /// <returns></returns>
    public static IQueryable<TEntity> Page<TEntity>(
        this IQueryable<TEntity> query,
        IPageBase request)
    {
        return query.Page(request.Page, request.Size);
    }

    /// <summary>
    ///     分页扩展
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="request">分页请求</param>
    /// <param name="count">数据规模</param>
    /// <returns></returns>
    public static IQueryable<TEntity> Page<TEntity>(
        this IQueryable<TEntity> query,
        IPageBase request,
        out long count)
    {
        return query.Page(request.Page, request.Size, out count);
    }

    /// <summary>
    ///     若条件为真则添加查询条件
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="condition">检查条件</param>
    /// <param name="predicate">查询条件</param>
    /// <returns></returns>
    public static IQueryable<TEntity> WhereIf<TEntity>(
        this IQueryable<TEntity> query,
        bool condition,
        Expression<Func<TEntity, bool>> predicate)
    {
        return condition ? query.Where(predicate) : query;
    }
}