using System;
using System.Linq;
using System.Linq.Expressions;
using FluentPaginator.Lib.Core.Interfaces;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace FluentPaginator.Lib.Core;

/// <summary>
/// The paginator class
/// </summary>
/// <typeparam name="T">The type of data in a page</typeparam>
public class Paginator<T> : IPaginator<T>
{
    private readonly IQueryable<T> _source;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="source">The IQueryable we will use for pagination</param>
    public Paginator(IQueryable<T> source)
    {
        _source = source;
    }

    /// <summary>
    /// Paginate a source of data
    /// </summary>
    /// <param name="paginationParameter">The parameter for page size and the current page</param>
    /// <param name="orderFunc">Function for how the elements will be ordered</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="TKey">The type used for ordering</typeparam>
    /// <returns>A page containing the data</returns>
    public Page<T> Paginate<TKey>(PaginationParameter paginationParameter, Expression<Func<T, TKey>>? orderFunc = null,
        PaginationOrder paginationOrder = PaginationOrder.Ascending)
    {
        var count = _source.Count();
        var (pageSize, pageNumber) = paginationParameter;
        var toSkip = (pageNumber - 1) * pageSize;
        if (orderFunc == null)
        {
            var items = _source
                .Skip(toSkip)
                .Take(pageSize)
                .ToList();
            var hasNext = count - pageSize * pageNumber > 0;
            return new Page<T>(items, pageNumber, pageSize, hasNext, count);
        }
        else
        {
            var itemsOrderTempResult = paginationOrder switch
            {
                PaginationOrder.Ascending  => _source.OrderBy(orderFunc),
                PaginationOrder.Descending => _source.OrderByDescending(orderFunc),
                _                          => throw new ArgumentOutOfRangeException(nameof(paginationOrder), paginationOrder, null)
            };
            
            var items = itemsOrderTempResult
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var hasNext = count - pageSize * pageNumber > 0;
            return new Page<T>(items, pageNumber, pageSize, hasNext, count);
        }
    }
}