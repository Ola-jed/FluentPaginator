using System;
using System.Collections.Generic;
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
///     The paginator
/// </summary>
/// <typeparam name="T">The type of data in a page</typeparam>
public class Paginator<T> : IPaginator<T>
{
    private readonly IQueryable<T> _source;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="source">The IQueryable we will use for pagination</param>
    public Paginator(IQueryable<T> source)
    {
        _source = source;
    }

    public Page<T> Paginate<TKey>(PaginationParameter paginationParameter, Expression<Func<T, TKey>>? orderFunc = null,
        PaginationOrder paginationOrder = PaginationOrder.Ascending, CountStrategy countStrategy = CountStrategy.Normal)
    {
        var count = countStrategy switch
        {
            CountStrategy.Normal => _source.Count(),
            CountStrategy.NoCount => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(countStrategy), countStrategy, null)
        };

        var (pageSize, pageNumber) = paginationParameter;
        var toSkip = (pageNumber - 1) * pageSize;
        IEnumerable<T> items;

        if (orderFunc == null)
        {
            items = _source
                .Skip(toSkip)
                .Take(pageSize + 1)
                .ToList();
        }
        else
        {
            var itemsOrderTempResult = paginationOrder switch
            {
                PaginationOrder.Ascending => _source.OrderBy(orderFunc),
                PaginationOrder.Descending => _source.OrderByDescending(orderFunc),
                _ => throw new ArgumentOutOfRangeException(nameof(paginationOrder), paginationOrder, null)
            };

            items = itemsOrderTempResult
                .Skip(toSkip)
                .Take(pageSize + 1)
                .ToList();
        }

        var hasNext = items.Count() > pageSize;
        return new Page<T>(items.Take(pageSize), pageNumber, pageSize, hasNext, count);
    }

    public async Task<Page<T>> AsyncPaginate<TKey>(PaginationParameter paginationParameter,
        Expression<Func<T, TKey>>? orderFunc = null,
        PaginationOrder paginationOrder = PaginationOrder.Ascending,
        CountStrategy countStrategy = CountStrategy.Normal,
        CancellationToken cancellationToken = default)
    {
        var count = countStrategy switch
        {
            CountStrategy.Normal => await _source.CountAsync(cancellationToken),
            CountStrategy.NoCount => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(countStrategy), countStrategy, null)
        };
        
        var (pageSize, pageNumber) = paginationParameter;
        var toSkip = (pageNumber - 1) * pageSize;
        IEnumerable<T> items;

        if (orderFunc == null)
        {
            items = await _source
                .Skip(toSkip)
                .Take(pageSize + 1)
                .ToListAsync(cancellationToken);
        }
        else
        {
            var itemsOrderTempResult = paginationOrder switch
            {
                PaginationOrder.Ascending => _source.OrderBy(orderFunc),
                PaginationOrder.Descending => _source.OrderByDescending(orderFunc),
                _ => throw new ArgumentOutOfRangeException(nameof(paginationOrder), paginationOrder, null)
            };

            items = await itemsOrderTempResult
                .Skip(toSkip)
                .Take(pageSize + 1)
                .ToListAsync(cancellationToken);
        }

        var hasNext = items.Count() > pageSize;
        return new Page<T>(items.Take(pageSize), pageNumber, pageSize, hasNext, count);
    }
}