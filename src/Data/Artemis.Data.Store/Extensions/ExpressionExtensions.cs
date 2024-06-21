using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Artemis.Data.Store.Extensions;

/// <summary>
///     表达式扩展
/// </summary>
public static class ExpressionExtensions
{
    /// <summary>
    ///     添加setter内容
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> AppendSetProperty<TEntity>(
        this Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> lhs,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> rhs)
    {
        var replace = new ReplacingExpressionVisitor(rhs.Parameters, new[] { lhs.Body });

        var combined = replace.Visit(rhs.Body);
        return Expression.Lambda<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>>(combined, lhs.Parameters);
    }
}