using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentPaginator.Lib.Core.Interfaces;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;

namespace FluentPaginator.Lib.Core;

/// <summary>
///     The cursor paginator implementation
/// </summary>
/// <typeparam name="T">The type of data in a page</typeparam>
public class CursorPaginator<T> : ICursorPaginator<T>
{
    private readonly IQueryable<T> _source;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="source">The IQueryable we will use for pagination</param>
    public CursorPaginator(IQueryable<T> source)
    {
        _source = source;
    }

    public CursorPage<T> Paginate<TKey>(
        CursorPaginationParameter paginationParameter,
        TKey? firstKey,
        Expression<Func<T, TKey>> orderFunc,
        PaginationOrder paginationOrder = PaginationOrder.Ascending,
        CountStrategy countStrategy = CountStrategy.Normal
    ) where TKey : struct, IComparable<TKey>
    {
        var count = countStrategy switch
        {
            CountStrategy.Normal => _source.Count(),
            CountStrategy.NoCount => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(countStrategy), countStrategy, null)
        };
        
        Expression<Func<T, bool>> filterExpression = x => true;
        if (firstKey.HasValue)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Invoke(orderFunc, parameter);
            var constant = Expression.Constant(firstKey.Value, typeof(TKey));
            Expression comparison = paginationOrder == PaginationOrder.Ascending
                ? Expression.GreaterThan(property, constant)
                : Expression.LessThan(property, constant);

            filterExpression = Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }

        var orderedQuery = paginationOrder == PaginationOrder.Ascending
            ? _source.OrderBy(orderFunc).Where(filterExpression)
            : _source.OrderByDescending(orderFunc).Where(filterExpression);

        var items = orderedQuery
            .Take(paginationParameter.PageSize)
            .ToList();

        return new CursorPage<T>(items, paginationParameter.PageSize, items.LastOrDefault(), count);
    }

    public async Task<CursorPage<T>> AsyncPaginate<TKey>(
        CursorPaginationParameter paginationParameter,
        TKey? firstKey,
        Expression<Func<T, TKey>> orderFunc,
        PaginationOrder paginationOrder = PaginationOrder.Ascending,
        CountStrategy countStrategy = CountStrategy.Normal,
        CancellationToken cancellationToken = default
    ) where TKey : struct, IComparable<TKey>
    {
        var count = countStrategy switch
        {
            CountStrategy.Normal => await _source.CountAsync(cancellationToken),
            CountStrategy.NoCount => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(countStrategy), countStrategy, null)
        };
        
        Expression<Func<T, bool>> filterExpression = x => true;
        if (firstKey.HasValue)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Invoke(orderFunc, parameter);
            var constant = Expression.Constant(firstKey.Value, typeof(TKey));
            Expression comparison = paginationOrder == PaginationOrder.Ascending
                ? Expression.GreaterThan(property, constant)
                : Expression.LessThan(property, constant);

            filterExpression = Expression.Lambda<Func<T, bool>>(comparison, parameter);
        }

        var orderedQuery = paginationOrder == PaginationOrder.Ascending
            ? _source.OrderBy(orderFunc).Where(filterExpression)
            : _source.OrderByDescending(orderFunc).Where(filterExpression);

        var items = await orderedQuery
            .Take(paginationParameter.PageSize)
            .ToListAsync(cancellationToken);

        return new CursorPage<T>(items, paginationParameter.PageSize, items.LastOrDefault(), count);
    }
}