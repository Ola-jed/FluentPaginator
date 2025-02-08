using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentPaginator.Lib.Core;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;

namespace FluentPaginator.Lib.Extensions;

/// <summary>
/// Extensions for IEnumerable
/// If you are using EF Core, you should use the <see cref="QueryableExtensions"/> instead
/// </summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Paginate an IEnumerable
    /// </summary>
    /// <param name="self">The IEnumerable to paginate</param>
    /// <param name="paginationParameter">The pagination param</param>
    /// <param name="orderFunc">Function for ordering items</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static Page<T> Paginate<T, TKey>(
        this IEnumerable<T> self,
        PaginationParameter paginationParameter,
        Expression<Func<T, TKey>>? orderFunc = null,
        PaginationOrder paginationOrder = PaginationOrder.Ascending
    )
    {
        return new Paginator<T>(self.AsQueryable()).Paginate(paginationParameter, orderFunc, paginationOrder);
    }

    /// <summary>
    /// Paginate an IEnumerable with urls
    /// </summary>
    /// <param name="self">The IEnumerable to paginate</param>
    /// <param name="paginationParameter">The url pagination param</param>
    /// <param name="orderFunc">Function for ordering items</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static UrlPage<T> UrlPaginate<T, TKey>(
        this IEnumerable<T> self,
        UrlPaginationParameter paginationParameter,
        Expression<Func<T, TKey>>? orderFunc = null,
        PaginationOrder paginationOrder = PaginationOrder.Ascending
    )
    {
        return new UrlPaginator<T>(self.AsQueryable()).Paginate(paginationParameter, orderFunc, paginationOrder);
    }

    /// <summary>
    /// Paginate an IEnumerable, using cursor-based pagination
    /// </summary>
    /// <param name="self">The IEnumerable to paginate</param>
    /// <param name="paginationParameter">The parameter for page size</param>
    /// <param name="firstKey">The starting item for the page</param>
    /// <param name="orderFunc">Function for how the elements will be ordered</param>
    /// <param name="paginationOrder">The order for ordering the items before paginating (Asc or Desc)</param>
    /// <typeparam name="T">The type of the page data</typeparam>
    /// <typeparam name="TKey">The type of the parameter for ordering</typeparam>
    /// <returns>The paginated result</returns>
    public static CursorPage<T> CursorPaginate<T, TKey>(
        this IEnumerable<T> self,
        CursorPaginationParameter paginationParameter,
        TKey? firstKey,
        Expression<Func<T, TKey>> orderFunc,
        PaginationOrder paginationOrder = PaginationOrder.Ascending
    ) where TKey : struct, IComparable<TKey>
    {
        return new CursorPaginator<T>(self.AsQueryable()).Paginate(paginationParameter, firstKey, orderFunc,
            paginationOrder);
    }
}