﻿using System.Linq.Expressions;
using Artemis.Data.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Artemis.Data.Store.Extensions;

/// <summary>
///     Queryable扩展
/// </summary>
public static class QueryableExtensions
{
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

    #region Page

    /// <summary>
    ///     分页扩展
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="page">页码(自1开始编码)</param>
    /// <param name="size">数据条目</param>
    /// <returns></returns>
    public static IQueryable<TEntity> Page<TEntity>(
        this IQueryable<TEntity> query,
        int? page,
        int? size)
    {
        if (page > 0 && size > 0)
            query = query.Skip((page.Value - 1) * size.Value);

        return size is > 0 ? query.Take(size.Value) : query;
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
        int? page,
        int? size,
        out long count)
    {
        count = query.LongCount();

        return query.Page(page, size);
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
    ///     映射式分页
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体键类型</typeparam>
    /// <typeparam name="TEntityInfo">映射类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="page">页码</param>
    /// <param name="size">条目数</param>
    /// <param name="cancellationToken">操作取消信号</param>
    /// <returns></returns>
    public static Task<List<TEntityInfo>> MapPageAsync<TEntity, TKey, TEntityInfo>(
        this IQueryable<TEntity> query,
        int? page,
        int? size,
        CancellationToken cancellationToken = default)
    {
        return query.Page(page, size)
            .ProjectToType<TEntityInfo>()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    ///     映射式分页
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <typeparam name="TKey">实体键类型</typeparam>
    /// <typeparam name="TEntityInfo">映射类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="request">页码</param>
    /// <param name="cancellationToken">条目数</param>
    /// <returns></returns>
    public static Task<List<TEntityInfo>> MapPageAsync<TEntity, TKey, TEntityInfo>(
        this IQueryable<TEntity> query,
        IPageBase request,
        CancellationToken cancellationToken = default)
    {
        return query.Page(request.Page, request.Size)
            .ProjectToType<TEntityInfo>()
            .ToListAsync(cancellationToken);
    }

    #endregion

    #region OrderBy

    /// <summary>
    ///     提交排序
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="propertyName">属性名</param>
    /// <param name="methodName">方法名</param>
    /// <returns></returns>
    private static IOrderedQueryable<TEntity> ApplyOrder<TEntity>(
        this IQueryable<TEntity> query,
        string propertyName,
        string methodName)
    {
        var parameter = Expression.Parameter(typeof(TEntity), "x");
        var property = Expression.Property(parameter, propertyName);
        var lambda = Expression.Lambda(property, parameter);
        var method = typeof(Queryable).GetMethods()
            .First(methodInfo => methodInfo.Name == methodName && methodInfo.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TEntity), property.Type);
        return (IOrderedQueryable<TEntity>)method.Invoke(null, new object[] { query, lambda })!;
    }

    /// <summary>
    ///     排序 ASC
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询条件</param>
    /// <param name="propertyName">属性名</param>
    /// <returns></returns>
    public static IOrderedQueryable<TEntity> OrderBy<TEntity>(
        this IQueryable<TEntity> query,
        string propertyName)
    {
        return query.ApplyOrder(propertyName, nameof(OrderBy));
    }

    /// <summary>
    ///     多字段排序ASC
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="propertyName">字段名</param>
    /// <returns></returns>
    public static IOrderedQueryable<TEntity> ThenBy<TEntity>(
        this IOrderedQueryable<TEntity> query,
        string propertyName)
    {
        return query.ApplyOrder(propertyName, nameof(ThenBy));
    }

    /// <summary>
    ///     排序 DESC
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询条件</param>
    /// <param name="propertyName">排序字段名</param>
    /// <returns></returns>
    public static IOrderedQueryable<TEntity> OrderByDescending<TEntity>(
        this IQueryable<TEntity> query,
        string propertyName)
    {
        return query.ApplyOrder(propertyName, nameof(OrderByDescending));
    }

    /// <summary>
    ///     多字段排序Desc
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="propertyName">字段名</param>
    /// <returns></returns>
    public static IQueryable<TEntity> ThenByDescending<TEntity>(
        this IOrderedQueryable<TEntity> query,
        string propertyName)
    {
        return query.ApplyOrder(propertyName, nameof(ThenByDescending));
    }

    #endregion
}