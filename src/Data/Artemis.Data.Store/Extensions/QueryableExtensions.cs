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
    public static IQueryable<TEntity> Page<TEntity>(this IQueryable<TEntity> query, int page, int size)
    {
        return query.Skip((page - 1) * size).Take(size);
    }
}