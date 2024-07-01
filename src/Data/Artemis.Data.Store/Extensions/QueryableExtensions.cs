using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Artemis.Data.Core;
using Artemis.Data.Core.Fundamental.Types;

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

    /// <summary>
    ///     装载查询定义
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    /// <param name="query">查询</param>
    /// <param name="definition">查询定义</param>
    /// <returns></returns>
    public static IQueryable<TEntity> SetUpDefinition<TEntity>(
        this IQueryable<TEntity> query,
        QueryDefinition definition)
    {
        if (definition.FilterDefinitions is not null && definition.FilterDefinitions.Count != 0)
            foreach (var filterDefinition in definition.FilterDefinitions)
            {
                var operation = "";

                if (filterDefinition.Operation.Equals(FilterOperationType.Equal))
                    operation = "==";
                else if (filterDefinition.Operation.Equals(FilterOperationType.NotEqual))
                    operation = "!=";
                else if (filterDefinition.Operation.Equals(FilterOperationType.GreaterThan))
                    operation = ">";
                else if (filterDefinition.Operation.Equals(FilterOperationType.GreaterThanOrEqual))
                    operation = ">=";
                else if (filterDefinition.Operation.Equals(FilterOperationType.LessThan))
                    operation = "<";
                else if (filterDefinition.Operation.Equals(FilterOperationType.LessThanOrEqual))
                    operation = "<=";
                else if (filterDefinition.Operation.Equals(FilterOperationType.Like)) operation = "Contains";

                if (operation == "") continue;

                var field = filterDefinition.Field;
                var value = filterDefinition.Value;

                query = query.Where($"{field} {operation} @0", value);
            }

        if (definition.SortDefinitions is not null && definition.SortDefinitions.Count != 0)
        {
            var sortDefinitions = definition.SortDefinitions
                .OrderBy(sort => sort.Index)
                .Select(sort => $"{sort.Field} {sort.Order.Name}");

            var sortPredicate = string.Join(",", sortDefinitions);

            query = query.OrderBy(sortPredicate).AsQueryable();
        }

        if (definition.UsePagination) query = query.Page(definition.Page, definition.Size);

        return query;
    }
}